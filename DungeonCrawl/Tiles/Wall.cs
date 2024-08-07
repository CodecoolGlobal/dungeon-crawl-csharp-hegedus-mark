namespace DungeonCrawl.Tiles;

    public class Wall : GameObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="hostingSurface"></param>
        public Wall(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.GreenYellow, Color.Transparent, '#'), position, hostingSurface)
        {
        }
    }
