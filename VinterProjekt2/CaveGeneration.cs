using Raylib_cs;
using System.Numerics;
public class CaveGeneration
{
    private float surfaceValue = 146f; //Desto högre talet är, desto mindre block genereras

    public readonly int worldSize = 100;

    private int tileSize = 100;

    private int Seed;

    Image noiseImage;

    Color alpha;

    private Texture2D perlinImage;

    public List<Tile> worldTiles = new();
    private List<Vector2> backgroundTiles = new();

    public CaveGeneration()
    {
        Seed = Random.Shared.Next(-1000, 1000);
        stoneWallpaper = Raylib.LoadTexture("Bilder/StoneWallpaper.png");
    }

    private Texture2D stoneWallpaper;

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
                    PlaceTile(new Stone(), new Vector2((int)x * tileSize, (int)y * tileSize));

                else if (alpha.r < surfaceValue && alpha.r > 80)
                    PlaceBackground(new Vector2((int)x * tileSize, (int)y * tileSize));


            }
        }

        Raylib.UnloadImage(noiseImage);
    }

    private void PlaceTile(Tile _tile, Vector2 _position)
    {
        _tile.Pos = _position;
        worldTiles.Add(_tile);
    }

    private void PlaceBackground(Vector2 _position)
    {
        backgroundTiles.Add(_position);
    }

    public void Draw()
    {
        for (int i = 0; i < worldTiles.Count; i++)
        {
            Raylib.DrawTexture(worldTiles[i].texture, (int)worldTiles[i].Pos.X, (int)worldTiles[i].Pos.Y, Color.WHITE);
        }
        for (int i = 0; i < backgroundTiles.Count; i++)
        {
            Raylib.DrawTexture(stoneWallpaper, (int)backgroundTiles[i].X, (int)backgroundTiles[i].Y, Color.WHITE);

        }
    }
}