namespace DungeonCrawl.Tiles;

public class Boss : GameObject
{
    public Boss(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Red, Color.Transparent, 'B'), position, hostingSurface)
    {
    }
    
    
}