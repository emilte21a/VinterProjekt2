using Raylib_cs;
using System.Numerics;
public class CaveGeneration
{
    private float surfaceValue = 121f;

    private int worldSize = 100;

    private int tileSize = 100;

    private int Seed;

    Image noiseImage;

    Color alpha;

    public Texture2D perlinImage;
    public List<Tile> worldTiles = new();

    //Instanser av tiles

    public CaveGeneration()
    {
        Seed = Random.Shared.Next(-1000, 1000);
    }

    public void GenerateTerrain()
    {
        noiseImage = Raylib.GenImagePerlinNoise(worldSize, worldSize, Seed, Seed, 10);
        perlinImage = Raylib.LoadTextureFromImage(noiseImage);


        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                alpha = Raylib.GetImageColor(noiseImage, x, y);
                if (alpha.r > surfaceValue)
                {
                    PlaceTile(new Stone(), new Vector2((int)x * tileSize, (int)y * tileSize));
                }
            }
        }

        Raylib.UnloadImage(noiseImage);
        
        // for (int x = 0; x < 5; x++)
        // {
        //     for (int y = 0; y < 5; y++)
        //     {

        //         PlaceTile(new Stone(), new Vector2((int)x * tileSize, (int)y * tileSize));
        //     }
        // }

    }

    private void PlaceTile(Tile _tile, Vector2 _position)
    {
        _tile.Pos = _position;
        worldTiles.Add(_tile);
    }

    public void Draw()
    {
        for (int i = 0; i < worldTiles.Count; i++)
        {
            //Raylib.DrawTexture(worldTiles[i].texture, (int)worldTiles[i].Pos.X, (int)worldTiles[i].Pos.Y, Color.WHITE);
            Raylib.DrawRectangleRec(worldTiles[i].tileRect,Color.BLACK);
        }

    }
}