using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.InventoryServices.Items;

public class UltimateWeapon : Item
{
    private const int COOLDOWN = 10;

    private static readonly ColoredGlyph Appearance =
        new ColoredGlyph(Color.Blue, Color.Transparent, 234);

    private readonly Direction[] _directions = new[]
    {
        Direction.Down, Direction.DownLeft, Direction.DownRight, Direction.Right, Direction.Left, Direction.Up,
        Direction.UpLeft, Direction.UpRight,
    };

    public UltimateWeapon() : base("Omega", Appearance)
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
                FastProjectile projectile = new FastProjectile(initialProjectilePosition, dir, map.SurfaceObject);
                map.AddMapObject(projectile);
            }
        }

        CurrentCooldownCounter = 0;
    }
}