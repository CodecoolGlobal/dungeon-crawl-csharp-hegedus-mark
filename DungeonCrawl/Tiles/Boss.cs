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
        private int count = 0;

        public Boss(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.Red, Color.Transparent, 'B'), position, hostingSurface)
        {
            BossHealth = 500;
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

        public void Shoot(Map map)
        {
            Direction direction = DetermineDirectionToPlayer(map);
            if (count < 60)
            {
                count++;
                return;
            }
            else
            {
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