using Raylib_cs;
using System.Numerics;
using System.Runtime.InteropServices;

public class Bullet
{
    float speed;

    public Rectangle bulletRec;

    Vector2 direction;

    public Vector2 position { get; private set; } //Varje skott har en egen position

    //Instans av ett skott där varje skott har privata positioner, riktningar, rektanglar och hastigheter
    public Bullet(Vector2 _position, Vector2 _direction, float _speed) 
    {
        position = _position; 
        this.direction = _direction;
        this.speed = _speed;
        bulletRec = new Rectangle(position.X, position.Y, 5, 5);
    }

    //Metod som uppdaterar skottets position
    public void Update()
    {
        position += direction * speed;
        bulletRec.x = position.X;
        bulletRec.y = position.Y;
    }

    //En lista med de tiles som skotten kolliderar med
    public List<Tile> collidingTiles;

    //Metod som kollar om ett skott borde förstöras eller inte
    public bool ShouldDestroy(Rectangle _playerRect, CaveGeneration cave)
    {
        collidingTiles = cave.TileToDestroy(cave, bulletRec);

        if (collidingTiles.Count > 0)
        {
            foreach (var colTile in collidingTiles)
                cave.worldTiles.Remove(colTile);
            
            return true;
        }

        else if (bulletRec.x < _playerRect.x - 1080 || bulletRec.x > _playerRect.x + 1080 || bulletRec.y < _playerRect.y - 1080 || bulletRec.y > _playerRect.y + 1080)
            return true;

        return false;
    }  

    //Metod som rituar ut skott
    public void Draw()
    {
        Raylib.DrawRectangleRec(bulletRec, Color.RED);
    }
}