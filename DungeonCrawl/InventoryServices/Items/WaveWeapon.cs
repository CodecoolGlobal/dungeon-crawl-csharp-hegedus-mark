using System;
using DungeonCrawl.Maps;
using DungeonCrawl.Tiles.MovableObjects;

namespace DungeonCrawl.InventoryServices.Items
{
    public class WaveWeapon : Item
    {
        private const int COOLDOWN = 60;

        private static readonly ColoredGlyph Appearance =
            new ColoredGlyph(Color.Cyan, Color.Transparent, 239);


        public WaveWeapon() : base("Wave", Appearance)
        {
        }

        public override void Use(Point playerPosition, Direction direction, Map map)
        {
            if (CurrentCooldownCounter < COOLDOWN)
            {
                return;
            }

            if (direction.Equals(Direction.Up))
            {
                CreateWave(Direction.Up, new[]
                {
                    playerPosition + Direction.Left,
                    playerPosition + Direction.Right,
                    playerPosition + Direction.Up
                }, map);
            }
            else if (direction.Equals(Direction.Down))
            {
                CreateWave(Direction.Down, new[]
                {
                    playerPosition + Direction.Left,
                    playerPosition + Direction.Right,
                    playerPosition + Direction.Down
                }, map);
            }
            else if (direction.Equals(Direction.Left))
            {
                CreateWave(Direction.Left, new[]
                {
                    playerPosition + Direction.Up,
                    playerPosition + Direction.Down,
                    playerPosition + Direction.Left
                }, map);
            }
            else if (direction.Equals(Direction.Right))
            {
                CreateWave(Direction.Right, new[]
                {
                    playerPosition + Direction.Up,
                    playerPosition + Direction.Down,
                    playerPosition + Direction.Right
                }, map);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            CurrentCooldownCounter = 0;
        }

        private void CreateWave(Direction projectileDirection, Point[] projectilePositions,
            Map map)
        {
            foreach (var pos in projectilePositions)
            {
                if (!map.TryGetMapObject(pos, out _))
                {
                    // Create and add the projectile to the map
                    Projectile projectile = new Projectile(pos, projectileDirection, map.SurfaceObject);
                    map.AddMapObject(projectile);
                }
            }
        }
    }
}