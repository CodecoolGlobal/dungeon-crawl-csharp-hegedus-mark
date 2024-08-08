using DungeonCrawl.InventoryServices;
using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.Tiles;

public class WeaponTile : GameObject
{
    private IItem _item;

    public WeaponTile(Point position, IScreenSurface hostingSurface, IItem item)
        : base(item.TileAppearance, position, hostingSurface)
    {
    }

    public override bool Touched(GameObject source, Map map)
    {
        if (source == map.UserControlledObject)
        {
            if (source is Player player)
            {
                player.PickUpWeapon(_item);
            }

            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }
}