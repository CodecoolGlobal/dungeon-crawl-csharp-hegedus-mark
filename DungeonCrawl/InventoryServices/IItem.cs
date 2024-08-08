using DungeonCrawl.Maps;

namespace DungeonCrawl.InventoryServices;

public interface IItem
{
    public void Use(Point initialPosition,Direction direction, Map map);
}