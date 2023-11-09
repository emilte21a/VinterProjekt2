using Raylib_cs;
using System.Numerics;
public class CaveGeneration
{
    private int surfaceValue = 100;

    private int getPixel;

    private int worldSize = 100;

    private int tileWidth = 1000;

    Image perlinImage;

    Texture2D texture;

    private List<Rectangle> worldTiles = new();

    public void GenerateTerrain()
    {
        surfaceValue = Random.Shared.Next(0, 201);

        for (int x = -worldSize; x < worldSize; x++)
        {
            for (int y = -worldSize; y < worldSize; y++)
            {
                getPixel = Random.Shared.Next(0, 201);

                if (getPixel > surfaceValue)
                {
                    worldTiles.Add(new Rectangle(x * tileWidth, y * tileWidth, tileWidth, tileWidth));
                }
            }
        }

    }

    public void Draw()
    {
        foreach (var tile in worldTiles)
        {
            Raylib.DrawRectangleRec(tile, Color.BLACK);
        }
    }
}