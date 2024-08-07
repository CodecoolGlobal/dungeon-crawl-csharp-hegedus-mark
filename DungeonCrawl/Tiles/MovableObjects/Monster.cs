using System;
using DungeonCrawl.Maps;
using DungeonCrawl.Ui;

namespace DungeonCrawl.Tiles.MovableObjects;

/// <summary>
/// Class <c>Monster</c> models a hostile object in the game.
/// </summary>
public class Monster : GameObject, IMovable
{
    public double Speed => 5;
    public int HealthPoint { get; private set; }
    private bool _monsterMovementSwitch;
    public Player Player;
    private double _accumulatedCell = 0.0;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Monster(Point position, IScreenSurface hostingSurface, Player player)
        : base(new ColoredGlyph(Color.Red, Color.Transparent, 'M'), position, hostingSurface)
    {
        HealthPoint = 100;
        Player = player;
        int attackPoint = 10;
    }

    public override bool Touched(GameObject source, Map map)
    {
        // Is the player the one that touched us?
        if (source == map.UserControlledObject)
        {
            new RootScreen().GameOver();
            return true;
        }

        if (source is Projectile)
        {
            map.RemoveMapObject(source);
            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }

    private void IsPlayerCloseToMonster(Map map)
    {
        int minDistance = 10; // Define the distance within which monsters start moving towards the player


        // Calculate the direction to move the monster one step closer to the player
        int moveX = Player.Position.X - Position.X;
        int moveY = Player.Position.Y - Position.Y;

        int stepX = moveX != 0 ? moveX / Math.Abs(moveX) : 0;
        int stepY = moveY != 0 ? moveY / Math.Abs(moveY) : 0;

        if (_monsterMovementSwitch && Math.Abs(stepX) == 1 && stepY == 0)
        {
            stepY = 1;
        }
        else if (!_monsterMovementSwitch && Math.Abs(stepX) == 1 && stepY == 0)
        {
            stepY = -1;
        }

        if (_monsterMovementSwitch && Math.Abs(stepY) == 1 && stepX == 0)
        {
            stepX = 1;
        }
        else if (!_monsterMovementSwitch && Math.Abs(stepY) == 1 && stepX == 0)
        {
            stepX = -1;
        }

        _monsterMovementSwitch = !_monsterMovementSwitch;

        Point newPosition = new Point(Position.X + stepX, Position.Y + stepY);


        if (Math.Abs(moveX) <= minDistance && Math.Abs(moveY) <= minDistance)
        {
            Move(newPosition, map);
        }
    }


    public void Update(TimeSpan timeElapsed, Map map)
    {
        if (_accumulatedCell > 1)
        {
            IsPlayerCloseToMonster(map);
            _accumulatedCell = 0.0;
            return;
        }

        _accumulatedCell += Speed * timeElapsed.TotalSeconds;
    }
}