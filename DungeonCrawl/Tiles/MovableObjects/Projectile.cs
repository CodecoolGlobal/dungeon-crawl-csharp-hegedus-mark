using System;
using DungeonCrawl.Maps;

namespace DungeonCrawl.Tiles.MovableObjects;

public class Projectile : GameObject, IMovable
{
    public Direction Direction;
    private double _accumulatedCell = 0.0;
    public virtual double Speed => 20;
    public int Attack;


    public Projectile(Point position, Direction direction, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Blue, Color.Transparent, '+'), position, hostingSurface)
    {
        Direction = direction;
        Attack = 10;
    }

    public override bool Move(Point newPosition, Map map)
    {
        if (!map.SurfaceObject.Surface.IsValidCell(newPosition.X, newPosition.Y))
        {
            map.RemoveMapObject(this);
            return false;
        }

        if (map.TryGetMapObject(newPosition, out GameObject foundObject))
        {
            // Handle collision specifically for projectiles
            if (foundObject.Touched(this, map))
            {
                // If the object is touched and interaction is allowed
                return true;
            }
            else
            {
                // If interaction is not allowed, remove the projectile
                map.RemoveMapObject(this);
                return false;
            }
        }


        DisplayMoveOnScreen(newPosition, map);
        return true;
    }
    
    public void Update(TimeSpan timeElapsed, Map map)
    {
        if (_accumulatedCell > 1)
        {
            var newPosition = Position + Direction;
            Move(newPosition, map);
            _accumulatedCell = 0.0;
            return;
        }

        _accumulatedCell += Speed * timeElapsed.TotalSeconds;
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