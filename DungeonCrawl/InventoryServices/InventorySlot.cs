using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace DungeonCrawl.InventoryServices;

public class InventorySlot
{
    public Item Item = null;
    public ScreenSurface SurfaceObject => _slotSurface;
    private Console _slotSurface;

    public InventorySlot(int width, int height, Point position)
    {
        _slotSurface = new Console(width, height);
        _slotSurface.Position = position;
        _slotSurface.DrawBox(new Rectangle(0, 0, width, height),
            ShapeParameters.CreateStyledBoxThin(Color.Gray));
    }

    public void AddItem(Item item)
    {
        ChangeItemGlyph(item);
        _slotSurface.IsDirty = true;
    }

    private void ChangeItemGlyph(Item item)
    {
        var x = _slotSurface.Surface.Area.Center.X;
        var y = _slotSurface.Surface.Area.Center.Y;
        var height = _slotSurface.Height;
        _slotSurface.Print(0, height - 1, item.Name, Color.Yellow);
        _slotSurface.SetCellAppearance(x, y, item.TileAppearance);
    }
}