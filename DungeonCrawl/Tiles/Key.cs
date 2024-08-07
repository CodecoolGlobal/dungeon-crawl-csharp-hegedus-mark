using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.Tiles;

public class Key : GameObject
{
    public Key(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Yellow, Color.Transparent, 'K'), position, hostingSurface)
    {
    }

    public override bool Touched(GameObject source, Map map)
    {
        if (source == map.UserControlledObject)
        {
            if (source is Player player)
            {
                player.PickUpKey(map);
            }
            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }
}