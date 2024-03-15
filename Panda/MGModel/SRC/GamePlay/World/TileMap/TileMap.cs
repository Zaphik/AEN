using System.IO;
using System.Linq;

namespace Panda.MGModel;

//Credit to that one guy in the year above for the idea to load from a csv file
// I did it better than him tho
public sealed class TileMap
{
    // the size of the tile and the grid
    private readonly Vector2 SlotSize;
    private readonly Vector2 GridSize;

    // the background and foreground tiles
    private readonly Tile[,] BackgroundTiles;
    private readonly Tile[,] ForegroundTiles;


    private GameServiceContainer Services { get; set; }

    public TileMap(Grid GRID, GameServiceContainer SERVICES)
    {
        Services = SERVICES;

        // sets the size of the tile and the grid from the grid
        SlotSize = GRID.Locsize;
        GridSize = GRID.GridSize;

        // sets the size of the background and foreground tiles
        BackgroundTiles = new Tile[GRID.Locs.GetLength(0), GRID.Locs.GetLength(1)];
        ForegroundTiles = new Tile[GRID.Locs.GetLength(0), GRID.Locs.GetLength(1)];

        // loads the background and foreground tiles from the tiled map
        LoadBackGroundFromTiled("2D/Tiles/Maps/Pandemonium_Background.csv", GRID);
        LoadForeGroundFromTiled("2D/Tiles/Maps/Pandemonium_Foreground.csv", GRID);
    }

    private void LoadBackGroundFromTiled(string PATH, Grid GRID)
    {
        // loads the background tiles from the tiled map
        var lines = File.ReadAllLines(Path.Combine(FileConsts.BaseDir, PATH)).ToList();

        // cycles through the lines and splits the tiles
        for (var i = 0; i < lines.Count; i++)
        {
            var tiles = lines[i].Split(',').ToList();

            // cycles through the tiles and sets the background tiles
            for (var j = 0; j < tiles.Count; j++)
            {
                if (j >= BackgroundTiles.GetLength(0) || i >= BackgroundTiles.GetLength(1)) continue;
                var tilePath = $"2D/Tiles/Background/{tiles[j]}.png";

                // checks if the tile is a wall, roof, door, pillar or house and fills the grid location
                if (tilePath.Contains("Roofs") || tilePath.Contains("Walls") || tilePath.Contains("door") ||
                    tilePath.Contains("pillar") || tilePath.Contains("Houses"))
                {
                    BackgroundTiles[j, i] =
                        new Tile(tilePath, GRID.Locs[j, i].pos, new Vector2(SlotSize.X, SlotSize.Y), Services);
                    GRID.Locs[j, i].Fill(true);
                }
                else
                {
                    BackgroundTiles[j, i] = new Tile(tilePath, GRID.Locs[j, i].pos,
                        new Vector2(SlotSize.X, SlotSize.Y), Services);
                }
            }
        }
    }

    public void LoadForeGroundFromTiled(string PATH, Grid GRID)
    {
        // loads the foreground tiles from the tiled map
        var lines = File.ReadAllLines(Path.Combine(FileConsts.BaseDir, PATH)).ToList();

        // cycles through the lines and splits the tiles
        for (var i = 0; i < lines.Count; i++)
        {
            var tiles = lines[i].Split(',').ToList();

            // cycles through the tiles and sets the foreground tiles
            for (var j = 0; j < tiles.Count; j++)
            {
                if (j >= ForegroundTiles.GetLength(0) || i >= ForegroundTiles.GetLength(1)) continue;
                var tilePath = $"2D/Tiles/Foreground/{tiles[j]}.png";


                // if it is a filler tile it continues
                if (tilePath.Contains("-1")) continue;

                // checks if the tile is a fence and fills the grid location
                if (tilePath.Contains("Fence"))
                {
                    ForegroundTiles[j, i] =
                        new Tile(tilePath, GRID.Locs[j, i].pos, new Vector2(SlotSize.X, SlotSize.Y), Services);
                    GRID.Locs[j, i].Fill(true);
                }
                else
                {
                    ForegroundTiles[j, i] = new Tile(tilePath, GRID.Locs[j, i].pos,
                        new Vector2(SlotSize.X, SlotSize.Y), Services);
                }
            }
        }
    }


    public void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        // draws the background and foreground tiles
        for (var i = 0; i < GridSize.X; i++)
        for (var j = 0; j < GridSize.Y; j++)
        {
            BackgroundTiles[i, j]?.Draw(OFFSET, SERVICES);
            ForegroundTiles[i, j]?.Draw(OFFSET, SERVICES);
        }
    }
}