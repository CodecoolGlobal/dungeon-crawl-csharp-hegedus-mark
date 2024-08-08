using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.Tiles;

public class SecretDoor : GameObject
{
    public SecretDoor(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.YellowGreen, Color.Transparent, '#'), position, hostingSurface)
    {
    }
    
    public override bool Touched(GameObject source, Map map)
    {
        if (source == map.UserControlledObject)
        {
            var player = (Player)source;
            
                if (player.Inventory.TreasuresCollected >= 1)
                {
                    map.RemoveMapObject(this);
                    return true;
                }
            
        }

        return false;
    }
}