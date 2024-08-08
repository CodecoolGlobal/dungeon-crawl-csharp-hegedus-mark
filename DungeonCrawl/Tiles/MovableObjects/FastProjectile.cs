namespace DungeonCrawl.Tiles.MovableObjects;

public class FastProjectile : Projectile
{
    public override double Speed => 60;

    public FastProjectile(Point position, Direction direction, IScreenSurface hostingSurface) : base(position,
        direction, hostingSurface)
    {
    }
}