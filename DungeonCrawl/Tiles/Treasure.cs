using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;
using SadConsole;
using SadRogue.Primitives;

namespace DungeonCrawl.Tiles;

/// <summary>
/// Class <c>Treasure</c> models a friendly object in the game.
/// </summary>
public class Treasure : GameObject
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Treasure(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Yellow, Color.Transparent, 15), position, hostingSurface)
    {
    }

    /// <param name="source"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public override bool Touched(GameObject source, Map map)
    {
        // Is the player the one that touched us?
        if (source is Player player)
        {
            player.CollectTreasure();
            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }
}