using System;
using System.Collections.Generic;
using DungeonCrawl.Maps;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using DungeonCrawl.InventoryServices;

namespace DungeonCrawl.Ui;

/// <summary>
/// Class <c>RootScreen</c> provides parent/child, components, and position.
/// </summary>
public class RootScreen : ScreenObject
{
    private Map _map;
    private int counter;

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

        var inventory = new Inventory(Game.Instance.ScreenCellsX - 10, 5);
        inventory.AddItem(testItem);
        var inventorySurface = inventory.SurfaceObject;
        inventorySurface.Position = new Point(0, Game.Instance.ScreenCellsY - 5);

        _map = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5, map1Walls);
        _map.DrawElementsOnConsole(5, 5);

        Children.Add(_map.SurfaceObject);
        Children.Add(inventorySurface);
        _map.DrawElementsOnConsole(5, 5);
    }

    public override void Update(TimeSpan timeElapsed)
    {
        base.Update(timeElapsed);

        counter++;
        System.Console.WriteLine($"Counter: {counter}");
        _map.MoveProjectiles();
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
            _map.UserControlledObject.Move(_map.UserControlledObject.Position + Direction.Up, _map);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Down))
        {
            _map.UserControlledObject.Move(_map.UserControlledObject.Position + Direction.Down, _map);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Left))
        {
            _map.UserControlledObject.Move(_map.UserControlledObject.Position + Direction.Left, _map);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            _map.UserControlledObject.Move(_map.UserControlledObject.Position + Direction.Right, _map);
            handled = true;
        }


        if (keyboard.IsKeyPressed(Keys.A))
        {
            _map.UserControlledObject.Shoot(Direction.Left, _map);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.D))
        {
            _map.UserControlledObject.Shoot(Direction.Right, _map);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.W))
        {
            _map.UserControlledObject.Shoot(Direction.Up, _map);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.S))
        {
            _map.UserControlledObject.Shoot(Direction.Down, _map);
            handled = true;
        }

        if (false)
        {
            _map.IsPlayerCloseToMonster();
        }

        return handled;
    }


    private List<(Point, Point)> map1Walls = new List<(Point, Point)>
    {
        (new Point(0, 0), new Point(37, 0)),
        (new Point(40,0),new Point(79,0)),
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
}