using System;
using DungeonCrawl.Maps;

namespace DungeonCrawl.Tiles.MovableObjects;

public class BossProjectTile : Projectile, IMovable
{
    private double _accumulatedCell = 0.0;
    public BossProjectTile(Point position, Direction direction, IScreenSurface hostingSurface) 
        : base(position, direction, hostingSurface)
    {
        this.Appearance = new ColoredGlyph(Color.Red, Color.Transparent, '+');
        this.Attack = 20;
    }
    
    public override bool Move(Point newPosition, Map map)
    {
        if (!map.SurfaceObject.Surface.IsValidCell(newPosition.X, newPosition.Y))
        {
            map.RemoveMapObject(this);
        }

        if (map.TryGetMapObject(newPosition, out GameObject foundObject))
        {
            
            if (foundObject.Touched(this, map))
            {
                return true;
            }
            else
            {
                map.RemoveMapObject(this);
                return false;
            }
        }


        DisplayMoveOnScreen(newPosition, map);
        return true;
    }
    
    public void HitSomething(GameObject source, Map map)
    {
        if (source is Player player)
        {
            player.Health -= this.Attack;
            if (player.Health <= 0)
            {
                map.RemoveMapObject(player);
                map._rootScreen.GameOver();
            }
        }

        map.RemoveMapObject(this);
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
}