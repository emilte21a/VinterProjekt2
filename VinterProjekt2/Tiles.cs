
using System.Drawing;
using System.Numerics;

public class Tile
{

    public Rectangle tile;
    public List<int> BoundsXY;

    public Vector2 pos;

    public Tile()
    {
        BoundsXY = new();
        tile = new((int)pos.X, (int)pos.Y, BoundsXY[0], BoundsXY[1]);
    }
}



public class Stone : Tile
{
    Tile tile = new();

    public Stone()
    {
        tile.BoundsXY = new(4);
        tile.pos = new Vector2(0, 0);
        tile.tile = new((int)pos.X, (int)pos.Y, BoundsXY[0], BoundsXY[1]);

    }
}