using System;
using DungeonCrawl.Maps;

namespace DungeonCrawl.InventoryServices;

public abstract class Item : IItem
{
    public ColoredGlyph TileAppearance { get; set; }
    public string Name { get; set; }

    protected Item(string name, ColoredGlyph tileAppearance)
    {
        Name = name;
        TileAppearance = tileAppearance;
    }

    public abstract void Use(Point playerPosition, Direction direction, Map map);
}