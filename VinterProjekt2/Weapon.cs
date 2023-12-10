using Raylib_cs;
using System.Numerics;
using System.Runtime.InteropServices;

public class Bullet
{
    float speed;
    Rectangle bulletRec;

    Vector2 direction;

    public Vector2 position { get; private set; }

    public Bullet(Vector2 _position, Vector2 _direction, float _speed)
    {
        position = _position;
        this.direction = _direction;
        this.speed = _speed;
        bulletRec = new Rectangle(position.X, position.Y, 5, 5);

    }

    public void Update()
    {
        position += direction * speed;
        bulletRec.x = position.X;
        bulletRec.y = position.Y;
    }

    public bool ShouldDestroy(Rectangle _playerRect)
    {
        if (bulletRec.x < _playerRect.x - 100 || bulletRec.x > _playerRect.x + 100 || bulletRec.y < _playerRect.y - 100 || bulletRec.y > _playerRect.y + 100)
            return true;

        else
            return false;
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(bulletRec, Color.RED);
    }
}