using DungeonCrawl.Maps;
using DungeonCrawl.Tiles;
using SadConsole.Host;
using SadConsole;
using SadConsole.Configuration;
using Game = SadConsole.Game;

namespace DungeonCrawl.Tests;

public class Tests
{
    private Map _map;

    [SetUp]
    public void Setup()
    {
        _map = new Map(80, 25);
        _map.DrawElementsOnConsole(5, 5);
        ScreenObject container = new ScreenObject();
        Builder gameStartup = new Builder()
            .SetScreenSize(80, 25)
            .IsStartingScreenFocused(true).OnStart(Startup);

        Game.Create(gameStartup);
        Game.Instance.Screen = container;
        container.Children.Add(_map.SurfaceObject);
    }


    [Test]
    public void DrawElements_Draws_N_Element_On_Console()
    {
        //_map.DrawElementsOnConsole(5, 5);
        var objects = _map.GameObjects;

        var monsters = 0;
        var treasures = 0;

        foreach (var element in objects)
        {
            if (element is Monster)
            {
                monsters++;
            }
            else if (element is Treasure)
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


    private void Startup(object? sender, GameHost host)
    {
        ScreenObject container = new ScreenObject();
        Game.Instance.Screen = container;

        container.Children.Add(_map.SurfaceObject);
    }
}