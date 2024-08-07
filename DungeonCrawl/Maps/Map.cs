using System;
using DungeonCrawl.Tiles;
using SadConsole;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawl.Maps
{
    public class Map
    {
        public IReadOnlyList<GameObject> GameObjects => _mapObjects.AsReadOnly();
        public ScreenSurface SurfaceObject => _mapSurface;
        public Player UserControlledObject { get; private set; }
        private List<GameObject> _mapObjects;
        private ScreenSurface _mapSurface;
        private Wall singleWall;
        private Wall singleWall1;

        public Map(int mapWidth, int mapHeight)
        {
            _mapObjects = new List<GameObject>();
            _mapSurface = new ScreenSurface(mapWidth, mapHeight);
            _mapSurface.UseMouse = false;

            UserControlledObject = new Player(_mapSurface.Surface.Area.Center, _mapSurface);
            CoordinatesOfWalls();
            for (int i = 0; i < 5; i++)
            {
                CreateTreasure();
                CreateMonster();
            }
            CreateWeapon();
            CreateKey();
            CreateDoor();
        }

        public void MoveProjectiles()
        {
            List<Shooting> Shoots = _mapObjects.OfType<Shooting>().ToList();

            foreach (Shooting shoot in Shoots)
            {
                bool hit = shoot.Move(this);
                if (hit)
                {
                    RemoveMapObject(shoot);
                }
            }
        }

        public void UpdateProjectiles()
        {
            List<Shooting> projectiles = _mapObjects.OfType<Shooting>().ToList();

            foreach (Shooting projectile in projectiles)
            {
                if (projectile.Move(this))
                {
                    RemoveMapObject(projectile);
                }
            }
        }

        public void AddMapObject(GameObject mapObject)
        {
            _mapObjects.Add(mapObject);
        }

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

        private void CreateWeapon()
        {
            for (int i = 0; i < 1000; i++)
            {
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

        private void CoordinatesOfWalls()
        {
            List<(Point, Point)> walls = new List<(Point, Point)>
            {
                (new Point(0, 0), new Point(37, 0)),
                (new Point(40, 0), new Point(79, 0)),
                (new Point(0, 1), new Point(0, 19)),
                (new Point(0, 19), new Point(79, 19)),
                (new Point(79, 0), new Point(79, 19)),

                (new Point(5, 2), new Point(5, 15)),
                (new Point(15, 4), new Point(15, 17)),
                (new Point(25, 2), new Point(25, 15)),
                (new Point(35, 4), new Point(35, 17)),
                (new Point(45, 2), new Point(45, 15)),
                (new Point(55, 4), new Point(55, 17)),
                (new Point(65, 2), new Point(65, 15)),

                (new Point(10, 3), new Point(20, 3)),
                (new Point(30, 6), new Point(40, 6)),
                (new Point(50, 9), new Point(60, 9)),
                (new Point(10, 12), new Point(20, 12)),
                (new Point(30, 15), new Point(40, 15)),
                (new Point(50, 18), new Point(60, 18)),
            };

            foreach (var (start, end) in walls)
            {
                CreateWall(start, end);
            }

            singleWall = new Wall(new Point(37, 1), _mapSurface);
            singleWall1 = new Wall(new Point(40, 1), _mapSurface);
            _mapObjects.Add(singleWall);
            _mapObjects.Add(singleWall1);
        }
    }
}
