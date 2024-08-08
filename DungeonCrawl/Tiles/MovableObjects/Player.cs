using System;
using System.Linq;
using DungeonCrawl.Maps;
using DungeonCrawl.Ui;

namespace DungeonCrawl.Tiles.MovableObjects
{
    public class Player : GameObject, IMovable
    {
        private bool _hasWeapon = false;
        private bool _hasKey = false;
        public double Speed => 10;
        public Direction Direction { get; set; }
        public bool Stopped { get; set; } = true;
        private double _accumulatedCell = 0.0;
        public int Health;

        public Player(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.Green, Color.Transparent, 2), position, hostingSurface)
        {
            Health = 100;
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

        public override bool Touched(GameObject source, Map map)
        {
            if (source is Weapon)
            {
                PickUpWeapon();
                return true;
            }

            if (source is Monster)
            {
                new RootScreen().GameOver();
                return false;
            }
            if (source is BossProjectTile bossProjectTile)
            {
                Health -= bossProjectTile.Attack;
                if (Health <= 0)
                {
                    map._rootScreen.GameOver();
                    
                }

                return false;
            }

            map.RemoveMapObject(this);

            return false;
        }


        public void Update(TimeSpan timeElapsed, Map map)
        {
            if (Stopped) return;
            if (_accumulatedCell > 1)
            {
                var newPosition = Position + Direction;
                Move(newPosition, map);
                _accumulatedCell = 0.0;
                return;
            }

            _accumulatedCell += Speed * timeElapsed.TotalSeconds;
        }
    }
}