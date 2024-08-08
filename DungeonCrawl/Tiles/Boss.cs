using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.Tiles;

public class Boss : GameObject
{
    public int BossHealth { get; private set; }
    public Boss(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Red, Color.Transparent, 'B'), position, hostingSurface)
    {
        BossHealth = 500;
    }
    
    public override bool Touched(GameObject source, Map map)
    {
        if (source is Projectile)
        {
            BossHealth -= 50;
            map.RemoveMapObject(source);
            if (BossHealth <= 0)
            {
                map.RemoveMapObject(this);
            }
            return true;
        }

        return false;
    }
    
    
    
}