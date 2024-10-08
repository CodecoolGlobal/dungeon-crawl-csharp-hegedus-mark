using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.InventoryServices.Items;

public class CircleWeapon : Item
{
    private const int COOLDOWN = 60;

    private static readonly ColoredGlyph Appearance =
        new ColoredGlyph(Color.LightBlue, Color.Transparent, 233);

    private readonly Direction[] _directions = new[]
    {
        Direction.Down, Direction.DownLeft, Direction.DownRight, Direction.Right, Direction.Left, Direction.Up,
        Direction.UpLeft, Direction.UpRight,
    };

    public CircleWeapon() : base("Circle", Appearance)
    {
    }
    

    public override void Use(Point playerPosition, Direction direction, Map map)
    {
        if (CurrentCooldownCounter < COOLDOWN)
        {
            return;
        }

        foreach (var dir in _directions)
        {
            var initialProjectilePosition = playerPosition + dir;
            if (!map.TryGetMapObject(initialProjectilePosition, out _))
            {
                // Create and add the projectile to the map
                Projectile projectile = new Projectile(initialProjectilePosition, dir, map.SurfaceObject, Color.LightBlue);
                map.AddMapObject(projectile);
            }
        }

        CurrentCooldownCounter = 0;
    }
}