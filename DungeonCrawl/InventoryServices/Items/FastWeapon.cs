using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.InventoryServices.Items;

public class FastWeapon : Item
{
    private const int COOLDOWN = 10;

    private static readonly ColoredGlyph Appearance =
        new ColoredGlyph(Color.Orange, Color.Transparent, 228);

    public FastWeapon() : base("Fast", Appearance)
    {
    }

    public override void Use(Point playerPosition, Direction direction, Map map)
    {
        if (CurrentCooldownCounter < COOLDOWN)
        {
            return;
        }

        var initialProjectilePosition = playerPosition + direction;
        if (!map.TryGetMapObject(initialProjectilePosition, out _))
        {
            // Create and add the projectile to the map
            FastProjectile projectile = new FastProjectile(initialProjectilePosition, direction, map.SurfaceObject);
            map.AddMapObject(projectile);
        }

        CurrentCooldownCounter = 0;
    }
}