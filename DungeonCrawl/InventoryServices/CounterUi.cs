using SadConsole.Quick;

namespace DungeonCrawl.InventoryServices;

public class CounterUi
{
    public Console Surface;

    private int _treasuresCollected = 0;

    public int TreasuresCollected
    {
        set
        {
            _treasuresCollected = value;
            Update();
        }
    }

    private static readonly ColoredGlyph treasureGlyph = new ColoredGlyph(Color.Yellow, Color.Transparent, 15);

    public CounterUi(int width, int height, Point position)
    {
        Surface = new Console(width, height);
        Surface.Position = position;
        
        Update();
    }

    private void Update()
    {
        var center = Surface.Surface.Area.Center;
        Surface.SetCellAppearance(center.X - 2, center.Y, treasureGlyph);
        
        
        Surface.Print(center.X , center.Y, $"x {_treasuresCollected}", Color.White);
        Surface.IsDirty = true;
    }
}