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
}
