using System;
using DungeonCrawl.Maps;

namespace DungeonCrawl.Tiles.MovableObjects;

public class Projectile : GameObject, IMovable
{
    public Direction Direction;
    public double Speed => throw new NotImplementedException();
 

    public Projectile(Point position, Direction direction, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Blue, Color.Transparent, '+'), position, hostingSurface)
    {
        Direction = direction;
    }

    public void HitSomething(GameObject source, Map map)
    {
        if (source is Monster)
        {
            map.RemoveMapObject(source);
        }

        map.RemoveMapObject(this);
    }
    
    public void Update(TimeSpan timeElapsed,  Map map)
    {
        return;
    }

    /*public  bool Move(Map map)
    {
        Point newPosition = Position + _direction;

        if (map.TryGetMapObject(newPosition, out GameObject foundObject))
        {
            switch (foundObject)
            {
                case Wall:
                    map.RemoveMapObject(this);
                    return true;
                case Treasure:
                    map.RemoveMapObject(this);
                    return true;
                case Monster:
                    map.RemoveMapObject(foundObject);
                    map.RemoveMapObject(this);
                    return true;

            }
        }

        RestoreMap(map);


        Position = newPosition;
        DrawGameObject(map.SurfaceObject);

        return false;
    }*/
}