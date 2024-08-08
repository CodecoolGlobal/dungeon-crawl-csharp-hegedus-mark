using System.Collections.Generic;
using System.Linq;

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


    public IEnumerable<IItem> Items => _items;
    private List<IItem> _items;
    private List<InventorySlot> _itemSlots;
    private InventorySlot _currentlySelectedSlot;

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
        _items = new List<IItem>(5);
        _itemSlots = new List<InventorySlot>(InventorySlotAmount);
        DrawItemSlots(InventorySlotAmount);
        DrawCounterUi();
    }

    private void DrawItemSlots(int slots)
    {
        for (int i = 0; i < slots; i++)
        {
            var slot = new InventorySlot(InventorySlotWidth, InventorySlotHeight,
                new Point(i * InventorySlotWidth, 0), i + 1);
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

    public IItem SelectItem(int index)
    {
        int safeIndex = index;
        if (safeIndex > _items.Count - 1)
        {
            safeIndex = 0;
        }

        var selectedItem = _items[safeIndex];
        _currentlySelectedSlot?.DeselectItemSlot();
        _currentlySelectedSlot = _itemSlots.FirstOrDefault(slot => slot.Item == selectedItem);
        _currentlySelectedSlot?.SelectItemSlot();
        return _items[safeIndex];
    }
}