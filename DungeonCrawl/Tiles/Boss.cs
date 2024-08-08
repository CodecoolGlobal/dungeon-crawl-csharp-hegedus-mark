using System;
using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;
using SadConsole;
using SadRogue.Primitives;

namespace DungeonCrawl.Tiles
{
    public class Boss : GameObject
    {
        public int BossHealth { get; private set; }
        private int _shootTimer;
        private readonly int _shootCooldown;

        public Boss(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.Red, Color.Transparent, 'B'), position, hostingSurface)
        {
            BossHealth = 500;
            _shootCooldown = 70; 
            _shootTimer = _shootCooldown;
        }

        public override bool Touched(GameObject source, Map map)
        {
            if (source is Projectile)
            {
                BossHealth -= 50;
                map.RemoveMapObject(source);
                if (BossHealth <= 0)
                {
                    map.RemoveMapObject(this);
                }

                return true;
            }

            return false;
        }

        public void UpdateBossShootMove(Map map)
        {
            if (_shootTimer > 0)
            {
                _shootTimer--;
            }

            if (_shootTimer == 0)
            {
                Shoot(map);
                MoveTowardsPlayer(map);
                _shootTimer = _shootCooldown;
            }
        }

        private void MoveTowardsPlayer(Map map)
        {
            int moveX = map.UserControlledObject.Position.X - Position.X;
            int moveY = map.UserControlledObject.Position.Y - Position.Y;

            int stepX = moveX != 0 ? moveX / Math.Abs(moveX) : 0;
            int stepY = moveY != 0 ? moveY / Math.Abs(moveY) : 0;

            Point newPosition = new Point(Position.X + stepX, Position.Y + stepY);

            if (Math.Abs(moveX) <= 70 && Math.Abs(moveY) <= 70)
            {
                Move(newPosition, map);
            }
        }
        
        

        public void Shoot(Map map)
        {
            Direction direction = DetermineDirectionToPlayer(map);
            if (direction != Direction.None)
            {
                Point initialPosition = Position + direction;
                if (!map.TryGetMapObject(initialPosition, out _))
                {
                    BossProjectTile projectile = new BossProjectTile(initialPosition, direction, map.SurfaceObject);
                    map.AddMapObject(projectile);
                }
            }
        }

        private Direction DetermineDirectionToPlayer(Map map)
        {
            int moveX = map.UserControlledObject.Position.X - Position.X;
            int moveY = map.UserControlledObject.Position.Y - Position.Y;

            if (Math.Abs(moveX) > Math.Abs(moveY))
            {
                return moveX > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return moveY > 0 ? Direction.Down : Direction.Up;
            }
        }
    }
}
