using DungeonCrawl.Maps;
using DungeonCrawl.Tiles;
using SadConsole.Host;
using SadConsole;
using Game = SadConsole.Host.Game;

namespace DungeonCrawl.Tests;

public class Tests
{
    private Map map;
    
    [SetUp]
    public void Setup()
    {
        map = new Map(80, 25);
    }


    [Test]
    public void DrawElements_Draws_N_Element_On_Console()
    {
        map.DrawElementsOnConsole(5,5);
        var objects = map.GameObjects;

        var monsters = 0;
        var treasures = 0;

        foreach (var element in objects)
        {
            if (element is Monster)
            {
                monsters++;
            } else if (element is Treasure)
            {
                treasures++;
            }
        }

        Assert.Multiple(() =>
        {
            Assert.That(monsters, Is.EqualTo(5));
            Assert.That(treasures, Is.EqualTo(5));
        });
    }
}