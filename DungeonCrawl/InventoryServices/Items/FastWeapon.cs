using DungeonCrawl.Maps;

namespace DungeonCrawl.InventoryServices.Items;

public class FastWeapon : Item
{
    public FastWeapon(string name, ColoredGlyph tileAppearance) : base(name, tileAppearance)
    {
    }

    public override void Use(Point playerPosition, Direction direction, Map map)
    {
        throw new System.NotImplementedException();
    }
}