using System;
using DungeonCrawl.Tiles;
using SadConsole;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace DungeonCrawl.Maps;

/// <summary>
/// Class <c>Map</c> models a map for the game.
/// </summary>
public class Map
{
    public IReadOnlyList<GameObject> GameObjects => _mapObjects.AsReadOnly();
    public ScreenSurface SurfaceObject => _mapSurface;
    public Player UserControlledObject { get; private set; }
    private List<GameObject> _mapObjects;
    private ScreenSurface _mapSurface;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mapWidth"></param>
    /// <param name="mapHeight"></param>
    public Map(int mapWidth, int mapHeight)
    {
        _mapObjects = new List<GameObject>();
        _mapSurface = new ScreenSurface(mapWidth, mapHeight);
        _mapSurface.UseMouse = false;

        UserControlledObject = new Player(_mapSurface.Surface.Area.Center, _mapSurface);
        CoordinatesOfWalls();
        // Create 5 treasure tiles and 5 monster tiles
        for (int i = 0; i < 5; i++)
        {
            CreateTreasure();
            CreateMonster();
        }
        CreateWeapon();

        
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

    /// <summary>
    /// Removes an object from the map.
    /// </summary>
    /// <param name="mapObject"></param>
    public void RemoveMapObject(GameObject mapObject)
    {
        if (_mapObjects.Contains(mapObject))
        {
            _mapObjects.Remove(mapObject);
            mapObject.RestoreMap(this);
        }
    }

    /// <summary>
    /// Creates a treasure on the map.
    /// </summary>
    private void CreateTreasure()
    {
        // Try 1000 times to get an empty map position
        for (int i = 0; i < 1000; i++)
        {
            // Get a random position
            Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

            // Check if any object is already positioned there, repeat the loop if found
            bool foundObject = _mapObjects.Any(obj => obj.Position == randomPosition);
            if (foundObject) continue;

            // If the code reaches here, we've got a good position, create the game object.
            GameObject treasure = new Treasure(randomPosition, _mapSurface);
            _mapObjects.Add(treasure);
            break;
        }
    }

    /// <summary>
    /// Creates a monster on the map.
    /// </summary>
    private void CreateMonster()
    {
        // Try 1000 times to get an empty map position
        for (int i = 0; i < 1000; i++)
        {
            // Get a random position
            Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

            // Check if any object is already positioned there, repeat the loop if found
            bool foundObject = _mapObjects.Any(obj => obj.Position == randomPosition);
            if (foundObject) continue;

            // If the code reaches here, we've got a good position, create the game object.
            GameObject monster = new Monster(randomPosition, _mapSurface);
            _mapObjects.Add(monster);
            break;
        }
    }

    private void CreateWeapon()
    {
        for (int i = 0; i < 1000; i++)
        {
            // Get a random position
            Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

            // Check if any object is already positioned there, repeat the loop if found
            bool foundObject = _mapObjects.Any(obj => obj.Position == randomPosition);
            if (foundObject) continue;

            // If the code reaches here, we've got a good position, create the game object.
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

    private void CoordinatesOfWalls()
    {
        
        List<(Point, Point)> walls = new List<(Point, Point)>
        {
            (new Point(0, 0), new Point(79, 0)),
            (new Point(0, 1), new Point(0, 19)),
            (new Point(0, 19), new Point(79, 19)),
            (new Point(79, 0), new Point(79, 19)),
            
           /* (new Point(10, 2), new Point(10, 17)),
            (new Point(20, 2), new Point(20, 17)),
            (new Point(30, 5), new Point(30, 14)),
            (new Point(40, 2), new Point(40, 17)),
            (new Point(50, 5), new Point(50, 14)),
            (new Point(60, 2), new Point(60, 17)),
            (new Point(70, 2), new Point(70, 17)),
        
            (new Point(15, 5), new Point(25, 5)),
            (new Point(35, 7), new Point(45, 7)),
            (new Point(55, 10), new Point(65, 10)),
            (new Point(15, 12), new Point(25, 12)),
            (new Point(35, 15), new Point(45, 15)), */
           
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
    }
}