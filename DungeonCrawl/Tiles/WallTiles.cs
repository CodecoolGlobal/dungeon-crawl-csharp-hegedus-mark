
using DungeonCrawl.Tiles;
using SadConsole;
using SadRogue.Primitives;

namespace DungeonCrawl.Maps
{
    public class Wall : GameObject
    {
        public Wall(Point position, ScreenSurface screenSurface)
            : base(new ColoredGlyph(Color.Gray, Color.Black, '#'), position, screenSurface)
        {
        }
    }
}