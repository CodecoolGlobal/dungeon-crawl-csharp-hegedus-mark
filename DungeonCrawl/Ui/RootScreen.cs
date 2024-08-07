using System;
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

        _map = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5);
        _map.DrawElementsOnConsole(5, 5);

        Children.Add(_map.SurfaceObject);
        Children.Add(inventorySurface);
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
        

        return handled;
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);
        
        counter++;
        System.Console.WriteLine($"Counter: {counter}");

    }
}