using System;
using DungeonCrawl.Maps;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace DungeonCrawl.Ui;

/// <summary>
/// Class <c>RootScreen</c> provides parent/child, components, and position.
/// </summary>
public class RootScreen : ScreenObject
{
    private Map _map;

    /// <summary>
    /// Constructor.
    /// </summary>
    public RootScreen()
    {
        _map = new Map(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5);

        Children.Add(_map.SurfaceObject);
    }
    public override void Update(TimeSpan timeElapsed)
    {
        base.Update(timeElapsed);

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

        if (keyboard.IsKeyPressed(Keys.Q))
        {
            _map.UserControlledObject.ShootLeft(_map);
            handled = true;
        }else if (keyboard.IsKeyPressed(Keys.E))
        {
            _map.UserControlledObject.ShootRight(_map);
            handled = true;
        }else if (keyboard.IsKeyPressed(Keys.W))
        {
            _map.UserControlledObject.ShootUp(_map);
            handled = true;
        }else if (keyboard.IsKeyPressed(Keys.S))
        {
            _map.UserControlledObject.ShootDown(_map);
            handled = true;
        }

        return handled;
    }
}