using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.InventoryServices.Items;

public class BasicWeapon : Item
{
    private const int COOLDOWN = 30;
    private int _currentCooldownCounter = 0;
    private static readonly ColoredGlyph Appearance = new ColoredGlyph(Color.Silver, Color.Transparent, 224);


    public BasicWeapon() : base("Basic", Appearance)
    {
    }

    public override void Use(Point playerPosition, Direction direction, Map map)
    {
        if (_currentCooldownCounter < COOLDOWN)
        {
            _currentCooldownCounter++;
            return;
        }

        var initialProjectilePosition = playerPosition + direction;
        if (!map.TryGetMapObject(initialProjectilePosition, out _))
        {
            // Create and add the projectile to the map
            Projectile projectile = new Projectile(initialProjectilePosition, direction, map.SurfaceObject);
            map.AddMapObject(projectile);
        }

        _currentCooldownCounter = 0;
    }
}