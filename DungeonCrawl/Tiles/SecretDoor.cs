using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.Tiles;

public class SecretDoor : GameObject
{
    public SecretDoor(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Pink, Color.Transparent, '#'), position, hostingSurface)
    {
    }
    
    public override bool Touched(GameObject source, Map map)
    {
        /*if (source == map.UserControlledObject)
        {
            if ()
            {
                map.RemoveMapObject(this);
                return true;
            }
        }*/

        return false;
    }
}