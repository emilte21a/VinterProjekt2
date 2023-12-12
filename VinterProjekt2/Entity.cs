
using Raylib_cs;
using System.Numerics;
using System.Dynamic;

public class Entity
{
    public Rectangle entityRect;
    public int Hp { get; set; }

    public float movementSpeed = 2.5f;

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

    public virtual void SpawnEntity(CaveGeneration cave)
    {

    }
}

public class Saucer : Entity
{
    UniversalMath uMath = new();
    Entity entity = new();
    bool isActive;
    public Saucer()
    {
        Hp = 4;
        entityRect = new Rectangle(0, 0, size, size);
        position = new Vector2(-100, 0);
        isActive = true;
    }

    public void Update(Vector2 _playerPos)
    {

        if (uMath.Distance(position, _playerPos) < 500)
            isActive = true;

        else if (uMath.Distance(position, _playerPos) > 700)
            isActive = false;

        if (isActive)
            EnemyMovement(_playerPos);

        else
            position = uMath.Lerp(position, position * 0.95f, 0.008f);
    }

    public override int Attack(int _damage)
    {
        return _damage;
    }

    private void EnemyMovement(Vector2 _playerPos)
    {
        position = uMath.Lerp(position, _playerPos, 0.01f);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(entityRect, Color.BLUE);
    }

    public override void SpawnEntity(CaveGeneration cave)
    {
        

    }

    private int AvailableSpawns(CaveGeneration cave)
    {
        for (int X = 0; X < cave.worldTiles.Count; X++)
        {
            for (int Y = 0; Y < cave.worldTiles.Count; Y++)
            {
                return 1;
            }
        }
        return 2;
    }
}


