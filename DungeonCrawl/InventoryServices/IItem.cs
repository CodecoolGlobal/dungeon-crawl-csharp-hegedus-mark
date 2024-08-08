using DungeonCrawl.Maps;

namespace DungeonCrawl.InventoryServices;

public interface IItem
{
    public void Use(Point initialPosition,Direction direction, Map map);

    public void RefreshCooldown();
    
    public ColoredGlyph TileAppearance { get; set; }
    public string Name { get; set; }
}