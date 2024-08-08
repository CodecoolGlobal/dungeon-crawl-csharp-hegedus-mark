using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.Tiles;

public class WeaponTile : GameObject
{
    public WeaponTile(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Blue, Color.Transparent, 15), position, hostingSurface)
    {
    }

    public override bool Touched(GameObject source, Map map)
    {
        if (source == map.UserControlledObject)
        {
            if (source is Player player)
            {
                player.PickUpWeapon();
            }
            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }
}