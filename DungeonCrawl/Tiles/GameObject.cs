﻿using DungeonCrawl.Maps;
using SadConsole;
using SadRogue.Primitives;

namespace DungeonCrawl.Tiles;

/// <summary>
/// Class <c>GameObject</c> models any objects in the game.
/// </summary>
public abstract class GameObject
{
    public Point Position { get; set; }
    public void RestoreMap(Map map) => _mapAppearance.CopyAppearanceTo(map.SurfaceObject.Surface[Position]);
    protected ColoredGlyph Appearance { get; set; }
    private ColoredGlyph _mapAppearance = new ColoredGlyph();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="appearance"></param>
    /// <param name="position"></param>
    /// <param name="hostingSurface"></param>
    protected GameObject(ColoredGlyph appearance, Point position, IScreenSurface hostingSurface)
    {
        Appearance = appearance;
        Position = position;

        // Store the map cell
        hostingSurface.Surface[position].CopyAppearanceTo(_mapAppearance);

        // Draw the object
        DrawGameObject(hostingSurface);
    }

    /// <summary>
    /// Moves the object to the given position on the map.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public virtual bool Move(Point newPosition, Map map)
    {
        // Check new position is valid
        if (!map.SurfaceObject.Surface.IsValidCell(newPosition.X, newPosition.Y)) return false;

        // Check if other object is there
        if (map.TryGetMapObject(newPosition, out GameObject foundObject))
        {
            // We touched the other object, but they won't allow us to move into the space
            if (!foundObject.Touched(this, map))
            {
                return false;
            }
        }
        var test = new ColoredGlyph(Color.Red, Color.Red);

        DisplayMoveOnScreen(newPosition, map);

        return true;
    }

    protected void DisplayMoveOnScreen(Point newPosition, Map map)
    {
        // Restore the old cell
        _mapAppearance.CopyAppearanceTo(map.SurfaceObject.Surface[Position]);

        // Store the map cell of the new position
        map.SurfaceObject.Surface[newPosition].CopyAppearanceTo(_mapAppearance);

        Position = newPosition;
        DrawGameObject(map.SurfaceObject);
    }

    /// <summary>
    /// Defines what should happen if another object touches the current one.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public virtual bool Touched(GameObject source, Map map)
    {
        return false;
    }


    /// <summary>
    /// Draws the object on the screen.
    /// </summary>
    /// <param name="screenSurface"></param>
    protected void DrawGameObject(IScreenSurface screenSurface)
    {
        Appearance.CopyAppearanceTo(screenSurface.Surface[Position]);
        screenSurface.IsDirty = true;
    }
}