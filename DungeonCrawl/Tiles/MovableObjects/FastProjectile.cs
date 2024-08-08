namespace DungeonCrawl.Tiles.MovableObjects;

public class FastProjectile : Projectile
{
    public override double Speed => 60;

    public FastProjectile(Point position,
        Direction direction,
        IScreenSurface hostingSurface,
        Color color,
        int attack = 10)
        : base(
            position,
            direction,
            hostingSurface,
            color,
            attack)
    {
    }
}