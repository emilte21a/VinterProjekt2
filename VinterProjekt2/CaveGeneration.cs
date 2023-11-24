using Raylib_cs;
using System.Numerics;
public class CaveGeneration
{
    private float surfaceValue = 121f;

    private int getPixel;

    private int worldSize = 240;

    private int tileWidth = 50;

    private int Seed;

    Image noise;

    Color alpha;

    public Texture2D perlinImage;
    public List<Rectangle> worldTiles = new();

    private Texture2D stoneTexture;

    public CaveGeneration()
    {
        Seed = Random.Shared.Next(-1000, 1000);
        stoneTexture = Raylib.LoadTexture("Bilder/Stone.png");
    }

    public void GenerateTerrain()
    {
        noise = Raylib.GenImagePerlinNoise(worldSize, worldSize, Seed, Seed, 10);

        perlinImage = Raylib.LoadTextureFromImage(noise);


        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                alpha = Raylib.GetImageColor(noise, x, y);
                getPixel = Random.Shared.Next(0, worldSize);
                System.Console.WriteLine(alpha.g);
                if (alpha.g > surfaceValue)
                {
                    worldTiles.Add(new Rectangle(x * tileWidth, y * tileWidth, tileWidth, tileWidth));
                }
            }
        }


        Raylib.UnloadImage(noise);

        // Raylib.ImageDraw(noise *, noise, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, 100, 100), Color.WHITE);

    }

    public void Draw()
    {
        foreach (var tile in worldTiles)
        {
            Raylib.DrawTexture(stoneTexture, (int)tile.x, (int)tile.y, Color.WHITE);
        }
    }
}