using DungeonCrawl.Maps;
using SadConsole;
using SadRogue.Primitives;

namespace DungeonCrawl.Tiles;

/// <summary>
/// Class <c>Player</c> models a user controlled object in the game.
/// </summary>
public class Player : GameObject
{
    private bool _hasWeapon = false;
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Player(Point position, IScreenSurface hostingSurface)
        : base(new ColoredGlyph(Color.Green, Color.Transparent, 2), position, hostingSurface)
    {
    }
    public void PickUpWeapon()
    {
        _hasWeapon = true;
    }

    public void Shoot(Direction direction, Map map)
    {
        if (_hasWeapon)
        {
            // Compute the initial position based on the direction
            Point initialPosition = Position + direction;

            // Check if the position is free of map objects
            if (!map.TryGetMapObject(initialPosition, out _))
            {
                // Create and add the projectile to the map
                Projectile projectile = new Projectile(initialPosition, direction, map.SurfaceObject);
                map.AddMapObject(projectile);
            }
        }
    }
    
    protected override bool Touched(GameObject source, Map map)
    {
        if (source is Weapon)
        {
            PickUpWeapon();
            return true;
        }

        return false;
    }
    
}