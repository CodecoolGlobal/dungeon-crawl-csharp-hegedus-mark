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
    private double HealthPoint { get; set; }
    private bool _monsterMovementSwitch;
    private int _monsterZigZagController = 1;
    private readonly Player _player;
    private double _accumulatedCell = 0.0;
    private bool _zigzag = true;
    private bool _chase = false;
    private const double StarterHp = 50;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    public Monster(Point position, IScreenSurface hostingSurface, Player player)
        : base(new ColoredGlyph(Color.Red, Color.Transparent, 'M'), position, hostingSurface)
    {
        HealthPoint = StarterHp;
        _player = player;
    }

    private Color ChangeColorAsPerHp(double hp)
    {
        double hpRatio = hp / StarterHp;
        System.Console.WriteLine(hpRatio);
        if (hpRatio <= 0.25)
        {
            this.Appearance.Foreground = Color.Blue;
            return Color.Blue;
        }
        else if (hpRatio <= 0.5)
        {
            this.Appearance.Foreground = Color.Purple;
            return Color.Purple;
        }
        else if (hpRatio <= 0.75)
        {
            this.Appearance.Foreground = Color.Yellow;
            return Color.Yellow;
        }

        return Color.Red;
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


            map.SurfaceObject.SetForeground(this.Position.X, this.Position.Y, ChangeColorAsPerHp(this.HealthPoint));
            map.SurfaceObject.IsDirty = true;
            return false;
        }

        return false;
    }

    private void IsPlayerCloseToMonster(Map map)
    {
        // Define the distance within which monsters start moving towards the player
        int minDistance = 10;
        // Calculate the direction to move the monster one step closer to the player
        int moveX = _player.Position.X - Position.X;
        int moveY = _player.Position.Y - Position.Y;
        
        //Knowing your direction it will make 1 movement towards you in the XY axis.
        int stepX = moveX != 0 ? moveX / Math.Abs(moveX) : 0;
        int stepY = moveY != 0 ? moveY / Math.Abs(moveY) : 0;
        
        //checks if the monster should move in ziggzagging motion
        if (_zigzag)
        {
            //Adds the ziggzagging motion to the monster
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

        //Motor that counts from 0 to 2 and then back
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

        //This code checks if the monster is clost to player. If close it stops the ziggzagg motion
        if (Math.Abs(map.UserControlledObject.Position.Y - this.Position.Y) <= 3 &&
            Math.Abs(map.UserControlledObject.Position.X - this.Position.X) <= 3)
        {
            _zigzag = false;
        }
        else
        {
            _zigzag = true;
        }

        //Create the new position of the monster that is one step closer to you
        Point newPosition = new Point(Position.X + stepX, Position.Y + stepY);

        //Checks if the monster is within minimal distance from you to start chasing you. Also it this code is responsible for the monster to keep continue chasing you, once you have triggered it.
        if (Math.Abs(moveX) <= minDistance && Math.Abs(moveY) <= minDistance || _chase)
        {
            _chase = true;
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