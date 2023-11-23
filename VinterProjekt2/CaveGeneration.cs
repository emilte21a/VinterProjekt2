using Raylib_cs;
using System.Numerics;
public class CaveGeneration
{
    private float surfaceValue = 0.3f;

    private int getPixel;

    private int worldSize = 240;

    private int tileWidth = 20;

    Image noise;

    Color alpha;

    public Texture2D perlinImage;
    private List<Rectangle> worldTiles = new();

    public void GenerateTerrain()
    {
        noise = Raylib.GenImagePerlinNoise(worldSize*tileWidth, worldSize*tileWidth, 0, 0, 1);


/*
        for (int x = -worldSize; x < worldSize; x++)
        {
            for (int y = -worldSize; y < worldSize; y++)
            {
                alpha = Raylib.GetImageColor(noise, x, y);
                //getPixel = Random.Shared.Next(0, 201);

                // if (alpha.a > surfaceValue)
                // {
                //     worldTiles.Add(new Rectangle(x * tileWidth, y * tileWidth, tileWidth, tileWidth));
                // }
            }
        }
        */
        perlinImage = Raylib.LoadTextureFromImage(noise);
        
        // Raylib.ImageDraw(noise *, noise, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, 100, 100), Color.WHITE);

    }

    public void Draw()
    {
        foreach (var tile in worldTiles)
        {
            Raylib.DrawRectangleRec(tile, Color.BLACK);
        }
    }
}