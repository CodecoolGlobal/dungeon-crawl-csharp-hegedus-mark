using System;
using System.Linq;
using DungeonCrawl.InventoryServices;
using DungeonCrawl.InventoryServices.Items;
using DungeonCrawl.Maps;
using DungeonCrawl.Ui;

namespace DungeonCrawl.Tiles.MovableObjects
{
    public class Player : GameObject, IMovable
    {
        public IItem CurrentlySelectedItem = new BasicWeapon("BasicWeapon",
            new ColoredGlyph(Color.Blue, Color.Transparent, 257),
            5);

        private bool _hasKey = false;
        public double Speed => 10;
        public Direction Direction { get; set; }
        public bool Stopped { get; set; } = true;
        private double _accumulatedCell = 0.0;
        public Inventory Inventory;
        public int Health;

        public Player(Point position, IScreenSurface hostingSurface, Inventory inventory)
            : base(new ColoredGlyph(Color.Green, Color.Transparent, 2), position, hostingSurface)
        {
            Inventory = inventory;
            Health = 100;
        }

        public void CollectTreasure()
        {
            Inventory.TreasuresCollected++;
            System.Console.WriteLine($"Treasures collected: {Inventory.TreasuresCollected}");
        }

        public void PickUpWeapon(IItem item)
        {
            Inventory.AddItem(item);
        }

        public void PickUpKey(Map map)
        {
            _hasKey = true;
            RemoveDoors(map);
        }

        private void RemoveDoors(Map map)
        {
            var doors = map.GameObjects.OfType<Door>().ToList();
            foreach (var door in doors)
            {
                map.RemoveMapObject(door);
            }
        }

        public bool HasKey => _hasKey;


        public void UseItem(Direction direction, Map map)
        {
            CurrentlySelectedItem.Use(Position, direction, map);
        }

        public override bool Touched(GameObject source, Map map)
        {
            if (source is Monster)
            {
                new RootScreen().GameOver();
                return false;
            }

            if (source is BossProjectTile bossProjectTile)
            {
                Health -= bossProjectTile.Attack;
                if (Health <= 0)
                {
                    map._rootScreen.GameOver();
                }

                return false;
            }

            map.RemoveMapObject(this);

            return false;
        }


        public void Update(TimeSpan timeElapsed, Map map)
        {
            if (Stopped) return;
            if (_accumulatedCell > 1)
            {
                var newPosition = Position + Direction;
                Move(newPosition, map);
                _accumulatedCell = 0.0;
                return;
            }

            _accumulatedCell += Speed * timeElapsed.TotalSeconds;
        }
    }
}