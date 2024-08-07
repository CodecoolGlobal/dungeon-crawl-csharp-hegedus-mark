using System;
using DungeonCrawl.Ui;
using SadConsole;
using SadConsole.Configuration;

namespace DungeonCrawl;

/// <summary>
/// Class <c>Program</c> provides an entry point for the game.
/// </summary>
public static class Program
{
    private const int ViewPortWidth = 80;
    private const int ViewPortHeight = 25;

    /// <summary>
    /// The entry point of the program.
    /// </summary>
    public static void Main()
    {
        // Create startup configuration for the engine.
        Builder startup = new Builder()
            .SetScreenSize(ViewPortWidth, ViewPortHeight)
            .SetStartingScreen<RootScreen>()
            .ConfigureFonts(true)
            .IsStartingScreenFocused(true);

        Game.Create(startup);
        Game.Instance.Run();
        Game.Instance.Dispose();

    }
    
    /* (new Point(10, 2), new Point(10, 17)),
             (new Point(20, 2), new Point(20, 17)),
             (new Point(30, 5), new Point(30, 14)),
             (new Point(40, 2), new Point(40, 17)),
             (new Point(50, 5), new Point(50, 14)),
             (new Point(60, 2), new Point(60, 17)),
             (new Point(70, 2), new Point(70, 17)),

             (new Point(15, 5), new Point(25, 5)),
             (new Point(35, 7), new Point(45, 7)),
             (new Point(55, 10), new Point(65, 10)),
             (new Point(15, 12), new Point(25, 12)),
             (new Point(35, 15), new Point(45, 15)), */
}
