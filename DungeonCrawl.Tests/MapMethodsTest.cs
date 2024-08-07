using DungeonCrawl.Maps;
using DungeonCrawl.Tiles;
using DungeonCrawl.Ui;
using SadConsole;
using Game = SadConsole.Host.Game;

namespace DungeonCrawl.Tests;

public class Tests
{
    private Map _map;

    [SetUp]
    public void Setup()
    {
        map = new Map(80, 25);
    }

        Game.Create(startup);
        Game.Instance.Run();
        _map = new Map(80, 25);
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
            Assert.That(monsters, Is.EqualTo(10));
            Assert.That(treasures, Is.EqualTo(5));
        });
    }
}