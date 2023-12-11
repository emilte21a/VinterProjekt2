
using Raylib_cs;
using System.Numerics;
using System.Dynamic;

public class Entity
{
    public Rectangle entityRect;
    public int Hp { get; set; }

    public int movementSpeed = 10;

    public Vector2 position
    {
        get => new Vector2(entityRect.x, entityRect.y);
        set
        {
            entityRect.x = value.X;
            entityRect.y = value.Y;
        }
    }

    private Texture2D entityTexture;

    public readonly int size = 50;

    public virtual int Attack(int _damage)
    {
        return 0;
    }

    public virtual void Draw()
    {

    }
}

public class Saucer : Entity
{
    bool isActive;
    public Saucer()
    {
        Hp = 100;
        entityRect = new Rectangle(-100, 0, size, size);
    }

    public void Update(Rectangle _playerRect)
    {
        // if (Math.Pow(entityRect.x - enemyTarget.x, 2) + Math.Pow(entityRect.y - enemyTarget.x, 2) < 20)
        //     isActive = true;
       
        // if (isActive)
        EnemyMovement(_playerRect);

        // if (Math.Pow(entityRect.x - enemyTarget.x, 2) + Math.Pow(entityRect.y - enemyTarget.x, 2) > 30)
        //     isActive = false;

    }

    public override int Attack(int _damage)
    {
        return _damage;
    }

    private void EnemyMovement(Rectangle _playerRect)
    {
        Vector2 playerPos = new Vector2(_playerRect.x, _playerRect.y);
        Vector2 diff = playerPos - new Vector2(entityRect.x, entityRect.y);
        Vector2 enemyDirection = Vector2.Normalize(diff);

        entityRect.x += enemyDirection.X * movementSpeed;
        entityRect.y += enemyDirection.Y * movementSpeed;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(entityRect, Color.BLUE);
    }
}


