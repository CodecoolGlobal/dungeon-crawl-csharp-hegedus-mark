using DungeonCrawl.Maps;
namespace DungeonCrawl.Tiles;

public class Weapon : GameObject
{
    public Weapon(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Blue, Color.Transparent, 15), position, hostingSurface)
    {
    }
    protected override bool Touched(GameObject source, Map map)
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