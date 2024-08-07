using System;
using DungeonCrawl.Maps;
using DungeonCrawl.Ui;
using SadConsole;
using SadRogue.Primitives;
using Game = SadConsole.Host.Game;

namespace DungeonCrawl.Tiles;

/// <summary>
/// Class <c>Monster</c> models a hostile object in the game.
/// </summary>
public class Monster : GameObject
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Monster(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Red, Color.Transparent, 'M'), position, hostingSurface)
    {
        int healthPoint = 100;
        int attackPoint = 10;
    }

    public override bool Touched(GameObject source, Map map)
    {
        // Is the player the one that touched us?
        if (source == map.UserControlledObject)
        {
            new RootScreen().GameOver();
            return true;
        }

        return false;
    }
}