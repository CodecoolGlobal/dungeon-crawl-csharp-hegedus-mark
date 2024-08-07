using SadConsole;
using SadRogue.Primitives;
using System.Collections.Generic;

namespace DungeonCrawl.Maps
{
    public class Walls
    {
        private ScreenSurface _screenSurface;
        private List<Wall> _walls;

        public Walls(ScreenSurface screenSurface)
        {
            _screenSurface = screenSurface;
            _walls = new List<Wall>();
        }

        public void InitializeWalls()
        {
            CreateRandomWalls(10); // Generating 10 random walls for example
        }

        public void CreateRandomWalls(int numberOfWalls)
        {
            for (int i = 0; i < numberOfWalls; i++)
            {
                CreateRandomWall();
            }
        }

        private void CreateRandomWall()
        {
            Point startPosition = new Point(Game.Instance.Random.Next(0, _screenSurface.Surface.Width),
                                            Game.Instance.Random.Next(0, _screenSurface.Surface.Height));

            int length = Game.Instance.Random.Next(3, 10); // Wall length between 3 and 10 tiles
            bool horizontal = Game.Instance.Random.Next(0, 2) == 0;

            for (int i = 0; i < length; i++)
            {
                Point position = horizontal
                    ? new Point(startPosition.X + i, startPosition.Y)
                    : new Point(startPosition.X, startPosition.Y + i);

                if (position.X >= _screenSurface.Surface.Width || position.Y >= _screenSurface.Surface.Height)
                    break;

                Wall wallSegment = new Wall(position, _screenSurface);
                _walls.Add(wallSegment);
                _screenSurface.Surface.SetCellAppearance(position.X, position.Y, wallSegment.Glyph);
            }
        }

        public bool IsWall(Point position)
        {
            foreach (var wall in _walls)
            {
                if (wall.Position == position)
                {
                    return true;
                }
            }
            return false;
        }
    }
}