using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DungeonCrawl.Maps;
using DungeonCrawl.Ui;

namespace DungeonCrawl.Tiles.MovableObjects;

/// <summary>
/// Class <c>Monster</c> models a hostile object in the game.
/// </summary>
public class Monster : GameObject, IMovable
{
    public double Speed => 5;
    public double HealthPoint { get; private set; }
    private bool _monsterMovementSwitch;
    private int _monsterZigZagController = 1;
    public Player Player;
    private double _accumulatedCell = 0.0;
    private bool zigzag = true;
    private bool chase = false;
    private double starterHp = 50;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Monster(Point position, IScreenSurface hostingSurface, Player player)
        : base(new ColoredGlyph(Color.Red, Color.Transparent, 'M'), position, hostingSurface)
    {
        HealthPoint = starterHp;
        Player = player;
        int attackPoint = 10;
    }

    private void ChangeColorAsPerHp(double hp)
    {
        double hpRatio = hp / starterHp;
        System.Console.WriteLine(hpRatio);
        if (hpRatio <= 0.25)
        {
            this.Appearance.Foreground = Color.Blue;
        }
        else if (hpRatio <= 0.5)
        {
            this.Appearance.Foreground = Color.Purple;
        }
        else if (hpRatio <= 0.75)
        {
            this.Appearance.Foreground = Color.Yellow;
        }
    }

    public override bool Touched(GameObject source, Map map)
    {
        // Is the player the one that touched us?
        if (source == map.UserControlledObject)
        {
            new RootScreen().GameOver();
            return true;
        }

        if (source is Projectile projectile)
        {
            HealthPoint -= projectile.Attack;
            map.RemoveMapObject(source);
            if (HealthPoint <= 0)
            {
                map.RemoveMapObject(this);
            }

            ChangeColorAsPerHp(this.HealthPoint);
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


        if (zigzag)
        {
            if (_monsterZigZagController == 0 && Math.Abs(stepX) == 1 && stepY == 0)
            {
                stepY = 1;
            }
            else if (_monsterZigZagController == 2 && Math.Abs(stepX) == 1 && stepY == 0)
            {
                stepY = -1;
            }
            else if (_monsterZigZagController == 0 && Math.Abs(stepY) == 1 && stepX == 0)
            {
                stepX = 1;
            }
            else if (_monsterZigZagController == 2 && Math.Abs(stepY) == 1 && stepX == 0)
            {
                stepX = -1;
            }
        }

        if (_monsterMovementSwitch)
        {
            _monsterZigZagController--;
        }
        else
        {
            _monsterZigZagController++;
        }

        if (_monsterZigZagController == 0)
        {
            _monsterMovementSwitch = false;
        }
        else if (_monsterZigZagController == 2)
        {
            _monsterMovementSwitch = true;
        }

        if (Math.Abs(map.UserControlledObject.Position.Y - this.Position.Y) <= 3 &&
            Math.Abs(map.UserControlledObject.Position.X - this.Position.X) <= 3)
        {
            zigzag = false;
        }
        else
        {
            zigzag = true;
        }

        Point newPosition = new Point(Position.X + stepX, Position.Y + stepY);

        if (Math.Abs(moveX) <= minDistance && Math.Abs(moveY) <= minDistance || chase)
        {
            chase = true;
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