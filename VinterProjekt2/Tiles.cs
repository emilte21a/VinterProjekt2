using System.Numerics;
using Raylib_cs;

public class Tile
{
    public Rectangle tileRect; 

    //Varje tiles position anges med vector2 istället för rektangelns position
    public Vector2 Pos
    {
        get => new Vector2(tileRect.x, tileRect.y);
        set
        {
            tileRect.x = value.X;
            tileRect.y = value.Y;
        }
    }
    public int tileSize = 100;
    public Texture2D texture;
}

public class Stone : Tile
{
    static Texture2D stoneImage; // En static texture2d vilket innebär att det endast finns en instans av bilden i hela programmet
    public Stone()
    {
        tileRect = new Rectangle(0, 0, tileSize, tileSize); 
        Pos = new Vector2(0, 0);

        if (stoneImage.id == 0)
            stoneImage = Raylib.LoadTexture("Bilder/Stone.png");

        texture = stoneImage;

        //Om stoneImage.id är 0
            //Ladda in stone.png som texture

        //Steninstansens textur är då stoneImage
        //Detta görs för att slippa behöva ladda in samma bild tusentals gånger när nya instanser av samma Stone klassen skapas
    }
}