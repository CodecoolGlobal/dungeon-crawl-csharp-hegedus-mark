using System.Collections.Generic;

namespace DungeonCrawl.InventoryServices;

public class Inventory
{
    public const int InventorySlotWidth = 9;
    public const int InventorySlotHeight = 5;
    public const int InventorySlotGap = 5;
    public const int InventorySlotAmount = 5;
    private List<IItem> _items;
    private List<InventorySlot> _itemSlots;
    public int TreasuresCollected;
    public ScreenSurface SurfaceObject => _inventorySurface;
    private ScreenSurface _inventorySurface;

    public Inventory(int width, int height)
    {
        _inventorySurface = new ScreenSurface(width, height);
        _items = new List<IItem>();
        _itemSlots = new List<InventorySlot>(InventorySlotAmount);
        DrawItemSlots(InventorySlotAmount);
    }

    private void DrawItemSlots(int slots)
    {
        for (int i = 0; i < slots; i++)
        {
            var slot = new InventorySlot(InventorySlotWidth, InventorySlotHeight,
                new Point(i * InventorySlotWidth, 0));
            _inventorySurface.Children.Add(slot.SurfaceObject);
            _itemSlots.Add(slot);
        }
    }

    public void AddItem(IItem item)
    {
        if (_items.Contains(item))
        {
            return;
        }

        int index = _items.Count;
        _items.Add(item);
        _itemSlots[index].AddItem(item);
    }

    public void RemoveItem(IItem item)
    {
        _items.Remove(item);
    }
}