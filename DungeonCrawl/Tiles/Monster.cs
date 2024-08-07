using DungeonCrawl.Maps;
using SadConsole;
using SadRogue.Primitives;
using Game = SadConsole.Host.Game;
using System;

namespace DungeonCrawl.Tiles;

/// <summary>
/// Class <c>Monster</c> models a hostile object in the game.
/// </summary>
public class Monster : GameObject
{
    public int HealthPoint { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Monster(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Red, Color.Transparent, 'M'), position, hostingSurface)
    {
        HealthPoint = 100;
        int attackPoint = 10;
    }

    protected override bool Touched(GameObject source, Map map)
    {
        if (source == map.UserControlledObject)
        {
            Environment.Exit(0);
        }

        if (source is Projectile)
        {
            map.RemoveMapObject(source);
            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }
}