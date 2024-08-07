using System;
using DungeonCrawl.Maps;
using DungeonCrawl.Ui;

namespace DungeonCrawl.Tiles.MovableObjects;

/// <summary>
/// Class <c>Monster</c> models a hostile object in the game.
/// </summary>
public class Monster : GameObject, IMovable
{
    public double Speed => 0.5;
    public int HealthPoint { get; private set; }
    private bool _monsterMovementSwitch;
    public Player Player;

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

    /*public void IsPlayerCloseToMonster()
    {
        int minDistance = 10; // Define the distance within which monsters start moving towards the player


        // Calculate the direction to move the monster one step closer to the player
        int moveX = Player.Position.X - monster.Position.X;
        int moveY = Player.Position.Y - monster.Position.Y;

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
            if (monster.Move(newPosition, this))
            {
                // Check if the monster has moved to the player's position
                if (newPosition == UserControlledObject.Position)
                {
                    UserControlledObject.Touched(monster, this);
                }
            }
        }
    }*/


    public void Update(TimeSpan timeElapsed, Map map)
    {
        return;
    }
}