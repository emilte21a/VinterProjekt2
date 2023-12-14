
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

    public virtual void Attack(int _damage, Player _player)
    {

    }

    public virtual void Draw()
    {

    }

    public virtual void SpawnEntity(Saucer _saucer, Vector2 _position)
    {

    }

    public enum EnemyDirection
    {
        RightUp,
        RightDown,
        LeftUp,
        LeftDown,
        Idle
    }

    public EnemyDirection GetEnemyDirection(Vector2 _playerPos, Saucer saucer)
    {
        Vector2 diff = saucer.position - _playerPos;
        Vector2 direction = Vector2.Normalize(diff);
        double radians = Math.Atan2(direction.Y, direction.X);

        radians *= (180 / Math.PI);

        if (radians > 90)
            return EnemyDirection.RightUp;

        else if (radians < 90 && radians > 0)
            return EnemyDirection.LeftUp;

        else if (radians < 0 && radians > -90)
            return EnemyDirection.LeftDown;

        else if (radians < -90)
            return EnemyDirection.RightDown;

        else
            return EnemyDirection.Idle;
    }

    public void CalculateCollisionSize(EnemyDirection _direction, Tile _collidingTile, Saucer saucer)
    {
        Rectangle collisionRec = Raylib.GetCollisionRec(saucer.entityRect, _collidingTile.tileRect);

        switch (_direction)
        {
            case EnemyDirection.RightUp:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    saucer.entityRect.y += (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    saucer.entityRect.x -= (int)collisionRec.width;

                break;

            case EnemyDirection.RightDown:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    saucer.entityRect.y -= (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    saucer.entityRect.x -= (int)collisionRec.width;

                break;

            case EnemyDirection.LeftUp:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    saucer.entityRect.y += (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    saucer.entityRect.x += (int)collisionRec.width;
                break;

            case EnemyDirection.LeftDown:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    saucer.entityRect.y -= (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    saucer.entityRect.x += (int)collisionRec.width;
                break;
        }
    }
}

public class Saucer : Entity
{
    UniversalMath uMath = new();
    int amountOfSaucers = 40;
    bool isActive;

    List<Saucer> saucers = new();

    System.Timers.Timer timer;

    public Saucer(Player player)
    {
        Hp = 4;
        entityRect = new Rectangle(0, 0, size, size);
        position = new Vector2(-100, 0);
        isActive = true;
        timer = new(interval: 1000);
        timer.Elapsed += (sender, e) => Attack(1, player);
    }

    public List<Tile> CheckCollision(CaveGeneration cave, Saucer saucer)
    {
        return cave.worldTiles.Where(worldTile => Raylib.CheckCollisionRecs(saucer.entityRect, worldTile.tileRect)).ToList(); //Returnerar en lista med de Tiles som fienden kolliderar med
    }

    List<Tile> collidingTiles;

    public void Update(Vector2 _playerPos, CaveGeneration cave, Player _player)
    {
        foreach (var saucer in saucers)
        {
            if (uMath.Distance(saucer.position, _playerPos) < 800)
                isActive = true;

            else if (uMath.Distance(saucer.position, _playerPos) > 1000)
                isActive = false;

            if (isActive)
                EnemyMovement(_playerPos, cave, saucer);

            else
                saucer.position = uMath.Lerp(saucer.position, saucer.position, 0.05f); // Om fienden inte är aktiv så ska den stanna.

            if (Raylib.CheckCollisionRecs(saucer.entityRect, _player.playerRect))
            {
                if (!timer.Enabled) //Kolla om timern inte är igång
                    timer.Start();//Starta timern
            }

            else
                timer.Stop();

        }
    }

    public override void Attack(int _damage, Player _player)
    {
        _player.hp -= _damage;
    }

    private void EnemyMovement(Vector2 _playerPos, CaveGeneration cave, Saucer saucer)
    {
        saucer.position = uMath.Lerp(saucer.position, _playerPos, 0.01f);

        collidingTiles = CheckCollision(cave, saucer);

        EnemyDirection direction = GetEnemyDirection(_playerPos, saucer);

        foreach (var colTile in collidingTiles)
        {
            if (colTile != null)
                CalculateCollisionSize(direction, colTile, saucer);
        }
    }

    public void GenerateEnemies(CaveGeneration cave, Player _player)
    {
        for (int x = 0; x < cave.tileGrid.GetLength(0); x++)
        {
            for (int y = 0; y < cave.tileGrid.GetLength(1); y++)
            {
                if (cave.tileGrid[x, y] == 0 && Random.Shared.Next(0, cave.worldSize * cave.worldSize) < 40 && saucers.Count < amountOfSaucers)
                    SpawnEntity(new Saucer(_player), new Vector2((int)x * cave.worldSize, (int)y * cave.worldSize));
            }
        }
    }

    public override void SpawnEntity(Saucer _saucer, Vector2 _position)
    {
        _saucer.position = _position;
        saucers.Add(_saucer);
    }

    public override void Draw()
    {
        for (int i = 0; i < saucers.Count; i++)
            Raylib.DrawRectangleRec(saucers[i].entityRect, Color.BLUE);

    }
}


