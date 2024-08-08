﻿using System;
using System.Collections.Generic;
using DungeonCrawl.Maps;
using SadConsole.Input;
using DungeonCrawl.InventoryServices;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace DungeonCrawl.Ui;

/// <summary>
/// Class <c>RootScreen</c> provides parent/child, components, and position.
/// </summary>
public class RootScreen : ScreenObject
{
    private Map _currentMap;
    private Inventory _inventory;
    private int counter;
    private bool menuSwitch = false;
    private Console menu;

    /// <summary>
    /// Constructor.
    /// </summary>
    public RootScreen()
    {
        var testItem = new Item
        {
            ForegroundColor = Color.Beige,
            GlyphIndex = 1,
            Name = "Test"
        };

        _inventory = new Inventory(Game.Instance.ScreenCellsX - 10, 5);
        _inventory.AddItem(testItem);

        _currentMap = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, map1Walls, this);
        _currentMap.DrawElementsOnConsole(5, 5);

        Children.Add(_currentMap.SurfaceObject);
        LoadInventory();
    }

    private void LoadInventory()
    {
        var inventorySurface = _inventory.SurfaceObject;
        inventorySurface.Position = new Point(0, Game.Instance.ScreenCellsY - 5);
        Children.Add(inventorySurface);
    }

    public void ChangeToMap2()
    {
        Children.Remove(_currentMap.SurfaceObject);
        _currentMap = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, map1Walls, this);
        _currentMap.DrawElementsOnConsole(1, 20);
        Children.Add(_currentMap.SurfaceObject);
    }

    public void ChangeToMap3()
    {
        Children.Remove(_currentMap.SurfaceObject);
        _currentMap = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, map1Walls, this);
        _currentMap.DrawElementsOnConsole(1, 20);
        Children.Add(_currentMap.SurfaceObject);
    }

    public override void Update(TimeSpan timeElapsed)
    {
        base.Update(timeElapsed);

        counter++;
        /*System.Console.WriteLine($"Counter: {counter}");*/
        var movables = _currentMap.Movables;

        foreach (var movableObject in movables)
        {
            movableObject.Update(timeElapsed, _currentMap);
        }
    }


    /// <summary>
    /// Processes keyboard inputs.
    /// </summary>
    /// <param name="keyboard"></param>
    /// <returns></returns>
    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        bool handled = HandlePlayerInteraction(keyboard);

        // For testing purposes
        if (keyboard.IsKeyPressed(Keys.NumPad1))
        {
            ChangeToMap2();
        }
        else if (keyboard.IsKeyPressed(Keys.Escape) && !menuSwitch)
        {
            menu = Menu();
            Game.Instance.Screen.Children.Add(menu);
            menuSwitch = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Escape) && menuSwitch)
        {
            System.Console.WriteLine("TURN OFF MENU");
            Game.Instance.Screen.Children.Remove(menu);
            menuSwitch = false;
        }

        return handled;
    }

    private bool HandlePlayerInteraction(Keyboard keyboard)
    {
        bool movementHandled = HandlePlayerMovement(keyboard);
        bool shootingHandled = HandlePlayerShoot(keyboard);

        return movementHandled || shootingHandled;
    }

    private bool HandlePlayerMovement(Keyboard keyboard)
    {
        bool handled = false;
        _currentMap.UserControlledObject.Stopped = false;

        bool up = keyboard.IsKeyDown(Keys.Up);
        bool down = keyboard.IsKeyDown(Keys.Down);
        bool left = keyboard.IsKeyDown(Keys.Left);
        bool right = keyboard.IsKeyDown(Keys.Right);

        if (up && left)
        {
            _currentMap.UserControlledObject.Direction = Direction.UpLeft;
            handled = true;
        }
        else if (up && right)
        {
            _currentMap.UserControlledObject.Direction = Direction.UpRight;
            handled = true;
        }
        else if (down && left)
        {
            _currentMap.UserControlledObject.Direction = Direction.DownLeft;
            handled = true;
        }
        else if (down && right)
        {
            _currentMap.UserControlledObject.Direction = Direction.DownRight;
            handled = true;
        }
        else if (up)
        {
            _currentMap.UserControlledObject.Direction = Direction.Up;
            handled = true;
        }
        else if (down)
        {
            _currentMap.UserControlledObject.Direction = Direction.Down;
            handled = true;
        }
        else if (left)
        {
            _currentMap.UserControlledObject.Direction = Direction.Left;
            handled = true;
        }
        else if (right)
        {
            _currentMap.UserControlledObject.Direction = Direction.Right;
            handled = true;
        }
        else
        {
            _currentMap.UserControlledObject.Stopped = true;
        }

        return handled;
    }

    private bool HandlePlayerShoot(Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.A))
        {
            _currentMap.UserControlledObject.Shoot(Direction.Left, _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.D))
        {
            _currentMap.UserControlledObject.Shoot(Direction.Right, _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.W))
        {
            _currentMap.UserControlledObject.Shoot(Direction.Up, _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.S))
        {
            _currentMap.UserControlledObject.Shoot(Direction.Down, _currentMap);
            handled = true;
        }


        return handled;
    }

    public ControlsConsole Menu()
    {
        // Create a ControlsConsole to manage UI elements
        var controlsConsole = new ControlsConsole(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);
        controlsConsole.Surface.Fill(Color.Black, Color.White, null);
        controlsConsole.Clear();

        // Print menu title directly on the ControlsConsole
        controlsConsole.Print(Game.Instance.ScreenCellsX / 2 - 2, Game.Instance.ScreenCellsY / 2 - 2, "Menu", Color.White);

        // Create a Quit button

        var quitButton = new Button(20, 1)
        {
            Text = "Quit",
            Position = new Point(Game.Instance.ScreenCellsX / 2 - 10, Game.Instance.ScreenCellsY / 2 + 2),
        };
        var saveButton = new Button(20, 1)
        {
            Text = "Save",
            Position = new Point(Game.Instance.ScreenCellsX / 2 - 10, Game.Instance.ScreenCellsY / 2 + 4)
        };


        // Attach the click event to quit the game
        quitButton.Click += (sender, e) =>
        {
            Environment.Exit(0);
        };

        // Add the button to the ControlsConsole
        controlsConsole.Controls.Add(quitButton);
        controlsConsole.Controls.Add(saveButton);

        // Return the ControlsConsole as the menu
        return controlsConsole;
    }


    private List<(Point, Point)> map1Walls = new List<(Point, Point)>
    {
        (new Point(0, 0), new Point(37, 0)),
        (new Point(40, 0), new Point(79, 0)),
        (new Point(0, 1), new Point(0, 19)),
        (new Point(0, 19), new Point(79, 19)),
        (new Point(79, 0), new Point(79, 19)),

        (new Point(5, 2), new Point(5, 15)),
        (new Point(15, 4), new Point(15, 17)),
        (new Point(25, 2), new Point(25, 15)),
        (new Point(35, 4), new Point(35, 17)),
        (new Point(45, 2), new Point(45, 15)),
        (new Point(55, 4), new Point(55, 17)),
        (new Point(65, 2), new Point(65, 15)),

        (new Point(10, 3), new Point(20, 3)),
        (new Point(30, 6), new Point(40, 6)),
        (new Point(50, 9), new Point(60, 9)),
        (new Point(10, 12), new Point(20, 12)),
        (new Point(30, 15), new Point(40, 15)),
        (new Point(50, 18), new Point(60, 18)),
    };

    public void GameOver()
    {
        // Create a new console to display the message
        var gameOverConsole = new Console(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);
        gameOverConsole.Print(Game.Instance.ScreenCellsX / 2 - 4, Game.Instance.ScreenCellsY / 2, "Game Over",
            Color.White);

        // Replace the current screen with the game over console
        Game.Instance.Screen = gameOverConsole;
    }
}