using System;
using System.Collections.Generic;
using DungeonCrawl.Maps;
using SadConsole.Input;
using DungeonCrawl.InventoryServices;
using SadConsole.UI;
using SadConsole.UI.Controls;
using DungeonCrawl.InventoryServices.Items;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.Ui;

/// <summary>
/// Class <c>RootScreen</c> provides parent/child, components, and position.
/// </summary>
public class RootScreen : ScreenObject
{
    private Map _currentMap;
    private Inventory _inventory;
    private int _counter;
    private bool _menuSwitch = false;
    private Console _menu;
    private Player _player;

    /// <summary>
    /// Constructor.
    /// </summary>
    public RootScreen()
    {
        var starterWeapon = new BasicWeapon();

        _inventory = new Inventory(Game.Instance.ScreenCellsX - 10, 5);
        _inventory.AddItem(starterWeapon);
        _inventory.SelectItem(0);

        _currentMap = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, _map1Walls, this);
        _player = new Player(_currentMap.SurfaceObject.Surface.Area.Center, _currentMap.SurfaceObject, _inventory);
        _player.CurrentlySelectedItem = _inventory.SelectItem(0);
        _currentMap.SpawnPlayer(_player);

        _currentMap.DrawElementsOnConsole(5, 5, RandomlyGeneratedItemToSpawn());
        Children.Add(_currentMap.SurfaceObject);
        LoadInventory();
    }

    private void LoadInventory()
    {
        var inventorySurface = _inventory.SurfaceObject;
        inventorySurface.Position = new Point(0, Game.Instance.ScreenCellsY - 5);
        Children.Add(inventorySurface);
    }

    public override void Update(TimeSpan timeElapsed)
    {
        base.Update(timeElapsed);
        if (_menuSwitch) return;
        _counter++;
        /*System.Console.WriteLine($"Counter: {counter}");*/
        var movables = _currentMap.Movables;
        _currentMap.MoveBoss();

        foreach (var movableObject in movables)
        {
            movableObject.Update(timeElapsed, _currentMap);
        }

        CheckPlayerPosition();
        RefreshItemCooldowns();
    }

    private void RefreshItemCooldowns()
    {
        foreach (var item in _inventory.Items)
        {
            item.RefreshCooldown();
        }
    }


    public void ChangeToMap2(Point newPlayerPosition)
    {
        Children.Remove(_currentMap.SurfaceObject);
        _currentMap = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, _map2Walls, this);
        SpawnPlayerOnMap(_currentMap, newPlayerPosition);
        _currentMap.DrawElementsOnConsole(1, 1, RandomlyGeneratedItemToSpawn());
        Children.Add(_currentMap.SurfaceObject);
    }

    public void ChangeToSecretMap(Point newPlayerPosition)
    {
        Children.Remove(_currentMap.SurfaceObject);
        _currentMap = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, _mapSecret, this);
        SpawnPlayerOnMap(_currentMap, newPlayerPosition);
        _currentMap.DrawElementsOnConsole(0, 0, new UltimateWeapon());
        Children.Add(_currentMap.SurfaceObject);
    }

    public void ChangeToMap3(Point newPlayerPosition)
    {
        Children.Remove(_currentMap.SurfaceObject);
        _currentMap = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, _map3Walls, this);
        SpawnPlayerOnMap(_currentMap, newPlayerPosition);
        _currentMap.DrawElementsOnConsole(0, 0, null, true);
        Children.Add(_currentMap.SurfaceObject);
    }

    private void SpawnPlayerOnMap(Map map, Point newPosition)
    {
        var currentlySelectedItem = _player.CurrentlySelectedItem;
        _player = new Player(newPosition, map.SurfaceObject, _inventory);
        _player.CurrentlySelectedItem = currentlySelectedItem;
        map.SpawnPlayer(_player);
    }

    public void CheckPlayerPosition()
    {
        var playerPos = _currentMap.UserControlledObject.Position;
        var currentMapWidth = _currentMap.SurfaceObject.Surface.Width;
        var currentMapHeight = _currentMap.SurfaceObject.Surface.Height;

        if (_currentMap.Walls == _map1Walls)
        {
            if (playerPos.Y == 0)
            {
                ChangeToMap2(new Point(playerPos.X, currentMapHeight - 2));
            }
        }
        else if (_currentMap.Walls == _map2Walls)
        {
            if (playerPos.X == 0)
            {
                ChangeToSecretMap(new Point(78, 10));
            }

            else if (playerPos.Y == 0)
            {
                ChangeToMap3(new Point(playerPos.X, currentMapHeight - 2));
            }
        }
        else if (_currentMap.Walls == _mapSecret)
        {
            if (playerPos.X == 79)
            {
                ChangeToMap2(new Point(1, 10));
            }
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


        if (keyboard.IsKeyPressed(Keys.Escape) && !_menuSwitch)
        {
            _menu = Menu();
            Game.Instance.Screen.Children.Add(_menu);
            _menuSwitch = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Escape) && _menuSwitch)
        {
            System.Console.WriteLine("TURN OFF MENU");
            Game.Instance.Screen.Children.Remove(_menu);
            _menuSwitch = false;
        }

        if (keyboard.IsKeyPressed(Keys.Home))
        {
            ChangeToMap3(new Point(40, 19));
        }

        return handled;
    }

    private bool HandlePlayerInteraction(Keyboard keyboard)
    {
        bool movementHandled = HandlePlayerMovement(keyboard);
        bool shootingHandled = HandlePlayerShoot(keyboard);
        bool inventoryHandled = HandlePlayerInventorySelection(keyboard);

        return movementHandled || shootingHandled || inventoryHandled;
    }

    private bool HandlePlayerMovement(Keyboard keyboard)
    {
        bool handled = false;

        Point newPosition = _currentMap.UserControlledObject.Position;
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


        if (keyboard.IsKeyDown(Keys.A))
        {
            _currentMap.UserControlledObject.UseItem(Direction.Left, _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyDown(Keys.D))
        {
            _currentMap.UserControlledObject.UseItem(Direction.Right, _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyDown(Keys.W))
        {
            _currentMap.UserControlledObject.UseItem(Direction.Up, _currentMap);
            handled = true;
        }
        else if (keyboard.IsKeyDown(Keys.S))
        {
            _currentMap.UserControlledObject.UseItem(Direction.Down, _currentMap);
            handled = true;
        }


        return handled;
    }

    private bool HandlePlayerInventorySelection(Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.NumPad1))
        {
            _player.CurrentlySelectedItem = _inventory.SelectItem(0);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.NumPad2))
        {
            _player.CurrentlySelectedItem = _inventory.SelectItem(1);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.NumPad3))
        {
            _player.CurrentlySelectedItem = _inventory.SelectItem(2);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.NumPad4))
        {
            _player.CurrentlySelectedItem = _inventory.SelectItem(3);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.NumPad5))
        {
            _player.CurrentlySelectedItem = _inventory.SelectItem(4);
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
        controlsConsole.Print(Game.Instance.ScreenCellsX / 2 - 2, Game.Instance.ScreenCellsY / 2 - 2, "Menu",
            Color.White);

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
        quitButton.Click += (sender, e) => { Environment.Exit(0); };

        // Add the button to the ControlsConsole
        controlsConsole.Controls.Add(quitButton);
        controlsConsole.Controls.Add(saveButton);

        // Return the ControlsConsole as the menu
        return controlsConsole;
    }

    public void GameOver()
    {
        // Create a new console to display the message
        var gameOverConsole = new Console(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);
        gameOverConsole.Print(Game.Instance.ScreenCellsX / 2 - 4, Game.Instance.ScreenCellsY / 2, "Game Over",
            Color.White);

        // Replace the current screen with the game over console
        Game.Instance.Screen = gameOverConsole;
    }

    private IItem RandomlyGeneratedItemToSpawn()
    {
        if (_spawnableItems.Count == 0)
        {
            return new BasicWeapon();
        }

        var randomIndex = Game.Instance.Random.Next(_spawnableItems.Count);
        var selectedItem = _spawnableItems[randomIndex];
        _spawnableItems.Remove(selectedItem);
        return selectedItem;
    }


    private List<(Point, Point)> _map1Walls = new List<(Point, Point)>
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


    private List<(Point, Point)> _map3Walls = new List<(Point, Point)>
    {
        (new Point(0, 0), new Point(79, 0)),
        (new Point(0, 1), new Point(0, 19)),
        (new Point(0, 19), new Point(37, 19)),
        (new Point(40, 19), new Point(79, 19)),
        (new Point(79, 0), new Point(79, 19)),
    };

    private List<(Point, Point)> _map2Walls = new List<(Point, Point)>
    {
        (new Point(0, 0), new Point(37, 0)),
        (new Point(40, 0), new Point(79, 0)),
        (new Point(0, 1), new Point(0, 9)),
        (new Point(0, 11), new Point(0, 19)),
        (new Point(0, 19), new Point(37, 19)),
        (new Point(40, 19), new Point(79, 19)),
        (new Point(79, 0), new Point(79, 19)),


        (new Point(10, 2), new Point(10, 17)),
        (new Point(20, 2), new Point(20, 17)),
        (new Point(30, 5), new Point(30, 14)),
        (new Point(40, 2), new Point(40, 17)),
        (new Point(50, 5), new Point(50, 14)),
        (new Point(60, 2), new Point(60, 17)),
        (new Point(70, 2), new Point(70, 17)),

        (new Point(15, 5), new Point(25, 5)),
        (new Point(35, 7), new Point(45, 7)),
        (new Point(55, 10), new Point(65, 10)),
        (new Point(15, 12), new Point(25, 12)),
        (new Point(35, 15), new Point(45, 15)),
    };

    private List<(Point, Point)> _mapSecret = new List<(Point, Point)>
    {
        (new Point(0, 0), new Point(79, 0)),
        (new Point(0, 1), new Point(0, 19)),
        (new Point(0, 19), new Point(79, 19)),
        (new Point(79, 0), new Point(79, 9)),
        (new Point(79, 11), new Point(79, 19)),
    };


    private List<IItem> _spawnableItems = new List<IItem>
    {
        new CircleWeapon(),
        new FastWeapon(),
        new WaveWeapon()
    };
}