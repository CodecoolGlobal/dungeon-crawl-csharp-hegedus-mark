using System;
using DungeonCrawl.Tiles;
using SadConsole;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawl.Maps
{

    /// <summary>
    /// Class <c>Map</c> models a map for the game.
    /// </summary>
    public class Map
    {
        public IReadOnlyList<GameObject> GameObjects => _mapObjects.AsReadOnly();
        public ScreenSurface SurfaceObject => _mapSurface;
        public Player UserControlledObject { get; private set; }
        private List<GameObject> _mapObjects;
        private List<GameObject> monsters => _mapObjects.Where(item => item is Monster).ToList();
        private ScreenSurface _mapSurface;
        private Wall singleWall;
        private Wall singleWall1;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        public Map(int mapWidth, int mapHeight, List<(Point, Point)> walls)
        {
            _mapObjects = new List<GameObject>();
            _mapSurface = new ScreenSurface(mapWidth, mapHeight);
            _mapSurface.UseMouse = false;

            UserControlledObject = new Player(_mapSurface.Surface.Area.Center, _mapSurface);
            CoordinatesOfWalls(walls);
        }

        public void DrawElementsOnConsole(int treasure, int monster)
        {
            for (int i = 0; i < treasure; i++)
            {
                CreateTreasure();
            }

            for (int i = 0; i < monster; i++)
            {
                CreateMonster();
            }

            CreateWeapon();
            CreateKey();
            CreateDoor();
        }

        public void MoveProjectiles()
        {
            List<Projectile> projectiles = _mapObjects.OfType<Projectile>().ToList();

            foreach (Projectile projectile in projectiles)
            {
                var newPosition = projectile.Position + projectile.Direction;


                if (TryGetMapObject(newPosition, out GameObject found))
                {
                    projectile.HitSomething(found, this);
                }
                else
                {
                    projectile.Move(newPosition, this);
                }
            }
        }


        public void AddMapObject(GameObject mapObject)
        {
            _mapObjects.Add(mapObject);
        }


        /// <summary>
        /// Try to find a map object at that position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool TryGetMapObject(Point position, out GameObject gameObject)
        {
            foreach (var otherGameObject in _mapObjects)
            {
                if (otherGameObject.Position == position)
                {
                    gameObject = otherGameObject;
                    return true;
                }
            }

            gameObject = null;
            return false;
        }

        public void RemoveMapObject(GameObject mapObject)
        {
            if (_mapObjects.Contains(mapObject))
            {
                _mapObjects.Remove(mapObject);
                mapObject.RestoreMap(this);
            }
        }

        private void CreateTreasure()
        {
            for (int i = 0; i < 1000; i++)
            {
                Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                    Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

                bool foundObject = _mapObjects.Any(obj => obj.Position == randomPosition);
                if (foundObject) continue;


                GameObject treasure = new Treasure(randomPosition, _mapSurface);
                _mapObjects.Add(treasure);
                break;
            }
        }

        private void CreateMonster()
        {
            for (int i = 0; i < 1000; i++)
            {
                Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                    Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

                bool foundObject = _mapObjects.Any(obj => obj.Position == randomPosition);
                if (foundObject) continue;


                GameObject monster = new Monster(randomPosition, _mapSurface);
                _mapObjects.Add(monster);
                break;
            }
        }


        public void IsPlayerCloseToMonster()
        {
            int minDistance = 10; // Define the distance within which monsters start moving towards the player

            foreach (var monster in monsters)
            {
                // Calculate the direction to move the monster one step closer to the player
                int moveX = UserControlledObject.Position.X - monster.Position.X;
                int moveY = UserControlledObject.Position.Y - monster.Position.Y;

                int stepX = moveX != 0 ? moveX / Math.Abs(moveX) : 0;
                int stepY = moveY != 0 ? moveY / Math.Abs(moveY) : 0;

                Point newPosition = new Point(monster.Position.X + stepX, monster.Position.Y + stepY);

                if (Math.Abs(moveX) <= minDistance && Math.Abs(moveY) <= minDistance)
                {
                    monster.Move(newPosition, this);
                }
            }
        }

        private void CreateWeapon()
        {
            for (int i = 0; i < 1000; i++)
            {
                // Get a random position
                Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                    Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

                bool foundObject = _mapObjects.Any(obj => obj.Position == randomPosition);
                if (foundObject) continue;

                GameObject Weapon = new Weapon(randomPosition, _mapSurface);
                _mapObjects.Add(Weapon);
                break;
            }
        }

        private void CreateWall(Point Start, Point End)
        {
            bool isHorizontal = Start.Y == End.Y;
            bool isVertical = Start.X == End.X;

            if (isHorizontal)
            {
                int minX = Math.Min(Start.X, End.X);
                int maxX = Math.Max(Start.X, End.X);
                for (int i = minX; i <= maxX; i++)
                {
                    Point position = new Point(i, Start.Y);
                    if (!_mapObjects.Any(o => o.Position == position))
                    {
                        GameObject Wall = new Wall(position, _mapSurface);
                        _mapObjects.Add(Wall);
                    }
                }
            }

            if (isVertical)
            {
                int minY = Math.Min(Start.Y, End.Y);
                int maxY = Math.Max(Start.Y, End.Y);
                for (int i = minY; i <= maxY; i++)
                {
                    Point position = new Point(Start.X, i);
                    if (!_mapObjects.Any(o => o.Position == position))
                    {
                        GameObject Wall = new Wall(position, _mapSurface);
                        _mapObjects.Add(Wall);
                    }
                }
            }
        }

        private void CoordinatesOfWalls(List<(Point, Point)> walls)
        {
            foreach (var (start, end) in walls)
            {
                CreateWall(start, end);
            }

            singleWall = new Wall(new Point(37, 1), _mapSurface);
            singleWall1 = new Wall(new Point(40, 1), _mapSurface);
            _mapObjects.Add(singleWall);
            _mapObjects.Add(singleWall1);

        }

        private void CreateKey()
        {
            for (int i = 0; i < 1000; i++)
            {
                Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                    Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

                bool foundObject = _mapObjects.Any(obj => obj.Position == randomPosition);
                if (foundObject) continue;


                GameObject Key = new Key(randomPosition, _mapSurface);
                _mapObjects.Add(Key);
                break;
            }
        }

        public void CreateDoor()
        {
            Point doorPosition = new Point((singleWall.Position.X + singleWall1.Position.X) / 2, singleWall.Position.Y);
            Point doorPosition1 = new Point(doorPosition.X + 1, singleWall.Position.Y);
            Door door1 = new Door(doorPosition1, _mapSurface);
            Door door = new Door(doorPosition, _mapSurface);
            _mapObjects.Add(door);
            _mapObjects.Add(door1);
        }

        
    }
}