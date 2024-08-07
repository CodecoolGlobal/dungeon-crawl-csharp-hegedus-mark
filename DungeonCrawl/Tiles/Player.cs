using System.Linq;
using DungeonCrawl.Maps;


namespace DungeonCrawl.Tiles
{
    public class Player : GameObject
    {
        private bool _hasWeapon = false;
        private bool _hasKey = false;

        public Player(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.Green, Color.Transparent, 2), position, hostingSurface)
        {
        }

        public void PickUpWeapon()
        {
            _hasWeapon = true;
        }

        public void PickUpKey(Map map)
        {
            _hasKey = true;
            RemoveDoors(map);
        }

        private void RemoveDoors(Map map)
        {
            var doors = map.GameObjects.OfType<Door>().ToList();
            foreach (var door in doors)
            {
                map.RemoveMapObject(door);
            }
        }

        public bool HasKey => _hasKey;


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
}