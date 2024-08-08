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
        private Inventory _inventory;

        public Player(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.Green, Color.Transparent, 2), position, hostingSurface)
        {
        }

        public void PickUpWeapon(IItem item)
        {
            return;
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