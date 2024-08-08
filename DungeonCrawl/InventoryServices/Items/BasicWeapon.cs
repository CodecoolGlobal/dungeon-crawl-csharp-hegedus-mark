using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.InventoryServices.Items;

public class BasicWeapon : Item
{
    public ColoredGlyph TileAppearance { get; set; }
    public string Name { get; set; }
    private int _projectileSpeed;


    public BasicWeapon(string name, ColoredGlyph tileAppearance, int projectileSpeed) : base(name, tileAppearance)
    {
        Name = name;
        TileAppearance = tileAppearance;
        _projectileSpeed = projectileSpeed;
    }

    public override void Use(Point playerPosition, Direction direction, Map map)
    {
        var initialProjectilePosition = playerPosition + direction;
        if (!map.TryGetMapObject(initialProjectilePosition, out _))
        {
            // Create and add the projectile to the map
            Projectile projectile = new Projectile(initialProjectilePosition, direction, map.SurfaceObject);
            map.AddMapObject(projectile);
        }
    }
}