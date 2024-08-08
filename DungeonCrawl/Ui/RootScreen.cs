using System;
using System.Collections.Generic;
using DungeonCrawl.Maps;
using SadConsole.Input;
using DungeonCrawl.InventoryServices;

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
            GlyphIndex = 6,
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

        _map = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, map1Walls);
        _map.DrawElementsOnConsole(5, 5);

        Children.Add(_map.SurfaceObject);
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
        System.Console.WriteLine($"Counter: {counter}");
        _currentMap.MoveProjectiles();
    }


    /// <summary>
    /// Processes keyboard inputs.
    /// </summary>
    /// <param name="keyboard"></param>
    /// <returns></returns>
    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Up))
        {
            _currentMap.UserControlledObject.Move(_currentMap.UserControlledObject.Position + Direction.Up,
                _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Down))
        {
            _currentMap.UserControlledObject.Move(_currentMap.UserControlledObject.Position + Direction.Down,
                _currentMap);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Left))
        {
            _currentMap.UserControlledObject.Move(_currentMap.UserControlledObject.Position + Direction.Left,
                _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            _currentMap.UserControlledObject.Move(_currentMap.UserControlledObject.Position + Direction.Right,
                _currentMap);
            handled = true;
        }


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
        else if (keyboard.IsKeyPressed(Keys.Escape) && !menuSwitch)
        {
            Menu();
            Game.Instance.Screen.Children.Add(menu);
            menuSwitch = true;

        }
        else if (keyboard.IsKeyPressed(Keys.Escape) && menuSwitch)
        {
            System.Console.WriteLine("TURN OFF MENU");
            Game.Instance.Screen.Children.Remove(menu);
            menuSwitch = false;
        }
        //For testing purposes
        if (keyboard.IsKeyPressed(Keys.NumPad1))
        {
            ChangeToMap2();
        }

        if (false)
        {
            _currentMap.IsPlayerCloseToMonster();
        }

        return handled;
    }

    public Console Menu()
    {
        menu = new Console(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);
        menu.Surface.DefaultBackground = Color.Black;
        menu.Print(Game.Instance.ScreenCellsX / 2 - 10, Game.Instance.ScreenCellsY / 2, "This is the menu", Color.White);
        return menu;
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