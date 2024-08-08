using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;
using SadConsole;
using SadRogue.Primitives;

namespace DungeonCrawl.Tiles
{
    public class Door : GameObject
    {
        public Door(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.Brown, Color.Transparent, '+'), position, hostingSurface)
        {
        }

        public override bool Touched(GameObject source, Map map)
        {
            if (source == map.UserControlledObject)
            {
                var player = (Player)source;
                if (player.HasKey)
                {
                    map.RemoveMapObject(this);
                    return true;
                }
            }

            return false;
        }
    }
}