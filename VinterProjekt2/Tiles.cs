using System.Numerics;
using Raylib_cs;

public class Tile
{
    public Rectangle tileRect;
    public Vector2 Pos {
        get => new Vector2(tileRect.x, tileRect.y);
        set {
            tileRect.x = value.X;
            tileRect.y = value.Y;
        }
    }
    public int tileSize = 50;
    public Texture2D texture;
}

public class Stone : Tile
{
    
    Texture2D stoneTexture = Raylib.LoadTexture("Bilder/Stone.png");
    public Stone()
    {
        tileRect = new Rectangle(0, 0, tileSize, tileSize);
        Pos = new Vector2(0, 0);
        texture = stoneTexture;
    }
}