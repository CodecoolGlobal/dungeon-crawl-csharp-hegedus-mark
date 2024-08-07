using System;
using DungeonCrawl.Maps;
using DungeonCrawl.Ui;
using SadConsole;
using SadRogue.Primitives;

namespace DungeonCrawl.Tiles;

/// <summary>
/// Class <c>Player</c> models a user controlled object in the game.
/// </summary>
public class Player : GameObject
{
    private bool _hasWeapon = false;
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Player(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Green, Color.Transparent, 2), position, hostingSurface)
    {
    }
    public void PickUpWeapon()
    {
        _hasWeapon = true;
    }

    public void ShootLeft(Map map)
    {
        if (_hasWeapon)
        {
            Point initialPosition = this.Position + Direction.Left;

            if (!map.TryGetMapObject(initialPosition, out _))
            {
                Projectile projectile = new Projectile(initialPosition, Direction.Left, map.SurfaceObject);
                map.AddMapObject(projectile);
            }
        }
    }
    public void ShootRight(Map map)
    {
        if (_hasWeapon)
        {
            Point initialPosition = this.Position + Direction.Right;

            if (!map.TryGetMapObject(initialPosition, out _))
            {
                Projectile projectile = new Projectile(initialPosition, Direction.Right, map.SurfaceObject);
                map.AddMapObject(projectile);
            }
        }
    }
    public void ShootUp(Map map)
    {
        if (_hasWeapon)
        {
            Point initialPosition = this.Position + Direction.Up;

            if (!map.TryGetMapObject(initialPosition, out _))
            {
                Projectile projectile = new Projectile(initialPosition, Direction.Up, map.SurfaceObject);
                map.AddMapObject(projectile);
            }
        }
    }
    public void ShootDown(Map map)
    {
        if (_hasWeapon)
        {
            Point initialPosition = this.Position + Direction.Down;

            if (!map.TryGetMapObject(initialPosition, out _))
            {
                Projectile projectile = new Projectile(initialPosition, Direction.Down, map.SurfaceObject);
                map.AddMapObject(projectile);
            }
        }
    }
    public override bool Touched(GameObject source, Map map)
    {
        if (source is Monster)
        {
            // Signal that the game is over
            new RootScreen().GameOver();
            return true;
        }
        return false;
    }
    
}