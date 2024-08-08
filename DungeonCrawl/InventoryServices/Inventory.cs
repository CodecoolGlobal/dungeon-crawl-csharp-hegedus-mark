using System.Collections.Generic;

namespace DungeonCrawl.InventoryServices;

public class Inventory
{
    public const int InventorySlotWidth = 10;
    public const int InventorySlotHeight = 5;
    public const int InventorySlotGap = 5;
    public const int InventorySlotAmount = 5;
    public const int CounterUiWidth = 10;
    public const int CounterUiHeight = 5;
    public static readonly Point CounterUiPosition = new Point(65, 0);


    private List<IItem> _items;
    private List<InventorySlot> _itemSlots;

    private int _treasuresCollected;

    public int TreasuresCollected
    {
        get => _treasuresCollected;
        set
        {
            _treasuresCollected = value;
            _counterUi.TreasuresCollected = value;
        }
    }

    public ScreenSurface SurfaceObject => _inventorySurface;
    private ScreenSurface _inventorySurface;
    private CounterUi _counterUi;

    public Inventory(int width, int height)
    {
        _inventorySurface = new ScreenSurface(width, height);
        _items = new List<IItem>();
        _itemSlots = new List<InventorySlot>(InventorySlotAmount);
        DrawItemSlots(InventorySlotAmount);
        DrawCounterUi();
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

    private void DrawCounterUi()
    {
        _counterUi = new CounterUi(CounterUiWidth, CounterUiHeight, CounterUiPosition);
        _inventorySurface.Children.Add(_counterUi.Surface);
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