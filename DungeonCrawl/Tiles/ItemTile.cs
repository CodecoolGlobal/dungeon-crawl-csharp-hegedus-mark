using DungeonCrawl.Maps;

namespace DungeonCrawl.Tiles;

public class ItemTile : GameObject
{
    public ItemTile(Point position, IScreenSurface hostingSurface) : base(new ColoredGlyph(Color.Aqua, Color.Crimson, 'I'),
        position, hostingSurface)
    {
    }

    public override bool Touched(GameObject source, Map map)
    {
        if (source == map.UserControlledObject)
        {
            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }
}