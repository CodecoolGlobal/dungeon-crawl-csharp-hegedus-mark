using System;
using DungeonCrawl.Tiles;
using SadConsole;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawl.Ui;

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
    private List<GameObject> monsters => _mapObjects.Where(item => item is Monster).ToList();
    private ScreenSurface _mapSurface;
    private RootScreen _rootScreen;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mapWidth"></param>
    /// <param name="mapHeight"></param>
    public Map(int mapWidth, int mapHeight, List<(Point, Point)> walls, RootScreen rootScreen)
    {
        _rootScreen = rootScreen;
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

    private void CoordinatesOfWalls(List<(Point, Point)> walls)
    {
        foreach (var (start, end) in walls)
        {
            CreateWall(start, end);
        }
    }
}