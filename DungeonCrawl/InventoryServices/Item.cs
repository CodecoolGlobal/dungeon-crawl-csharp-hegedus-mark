using System;
using DungeonCrawl.Maps;

namespace DungeonCrawl.InventoryServices;

public abstract class Item : IItem
{
    protected int CurrentCooldownCounter = 0;
    public ColoredGlyph TileAppearance { get; set; }
    public string Name { get; set; }

    protected Item(string name, ColoredGlyph tileAppearance)
    {
        Name = name;
        TileAppearance = tileAppearance;
    }

    public abstract void Use(Point playerPosition, Direction direction, Map map);

    public void RefreshCooldown()
    {
        CurrentCooldownCounter++;
    }
}