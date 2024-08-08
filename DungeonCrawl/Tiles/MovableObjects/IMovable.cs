using System;
using DungeonCrawl.Maps;

namespace DungeonCrawl.Tiles;

public interface IMovable
{
    double Speed { get; }
    void Update(TimeSpan timeElapsed, Map map);
}