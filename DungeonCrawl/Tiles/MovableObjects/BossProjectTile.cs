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
    
    
}