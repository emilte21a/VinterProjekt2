
using Raylib_cs;
using System.Numerics;
using System.Dynamic;

public class Entity
{
    //Fiende Rektangel
    public Rectangle entityRect;

    //Fiendens position anges som en vector2 istället för en rektangels position
    public Vector2 position
    {
        get => new Vector2(entityRect.x, entityRect.y);
        set
        {
            entityRect.x = value.X;
            entityRect.y = value.Y;
        }
    }

    //FiendeRektangelns storlek
    public readonly int size = 50;

    //Virtuell metod som tillåter överskridande om hur fienden ska ritas ut för olika sorters fiender
    public virtual void Draw()
    {

    }

    //En enum med de olika riktningar som fienden kan ha
    public enum EnemyDirection
    {
        RightUp,
        RightDown,
        LeftUp,
        LeftDown,
        Idle
    }

    //Metod som returnerar riktningen som fienden rör sig
    public EnemyDirection GetEnemyDirection(Vector2 playerPos, Saucer saucer)
    {
        Vector2 diff = saucer.position - playerPos;
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

    //Metod som räknar ut storleken av kollisionsrektanglar med världsblocks
    public void CalculateCollisionSize(EnemyDirection direction, Tile collidingTile, Saucer saucer)
    {
        Rectangle collisionRec = Raylib.GetCollisionRec(saucer.entityRect, collidingTile.tileRect);

        switch (direction)
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

public class Saucer : Entity //Saucer är en typ av en Entity
{
    UniversalMath uMath = new();

    //Mängden fiender som ska finnas
    int amountOfSaucers = 40; 

    //Kollar om fienden är inom ett visst område av spelaren
    bool isWithinRangeOfPlayer;

    //Lista med alla fiender
    List<Saucer> saucers = new();

    public Saucer()
    {
        //Instansera varje fiendes storlek
        entityRect = new Rectangle(0, 0, size, size); 
        position = new Vector2(-100, 0);
        isWithinRangeOfPlayer = true;
    }

    //Metod som returnerar sen lista med de worldtiles som fienden kolliderar med
    public List<Tile> CheckCollision(CaveGeneration cave, Saucer saucer)
    {
        return cave.worldTiles.Where(worldTile => Raylib.CheckCollisionRecs(saucer.entityRect, worldTile.tileRect)).ToList(); //Returnerar en lista med de Tiles som fienden kolliderar med
    }

    List<Tile> collidingTiles;

    //Metod som uppdaterar om fienden borde röra på sig eller inte
    public void Update(Vector2 playerPos, CaveGeneration cave, Player player)
    {
        foreach (var saucer in saucers)
        {
            if (uMath.Distance(saucer.position, playerPos) < 800)
                isWithinRangeOfPlayer = true;

            else if (uMath.Distance(saucer.position, playerPos) > 1000)
                isWithinRangeOfPlayer = false;

            if (isWithinRangeOfPlayer)
                EnemyMovement(playerPos, cave, saucer, player);

            else
                saucer.position = uMath.Lerp(saucer.position, saucer.position, 1.5f); // Om fienden inte är aktiv så blir den idle.
        }
    }

    //Metod som returnerar hur mycket skada ett "slag" från fienden gör
    public int Attack(int _damage)
    {
        return _damage;
    }

    private bool isTimerActive = false; //Bool som kollar om fienden kolliderar med spelaren
    private int damageTimer = 1; 

    //Metod som uppdaterar fiendens rörelse
    private void EnemyMovement(Vector2 playerPos, CaveGeneration cave, Saucer saucer, Player player)
    {
        saucer.position = uMath.Lerp(saucer.position, playerPos, 0.01f); //Uppdatera fiendens position så att den följer spelaren

        collidingTiles = CheckCollision(cave, saucer); //Lista med de worldtiles som fienden kolliderar med 

        EnemyDirection direction = GetEnemyDirection(playerPos, saucer); //Fiendens riktning

        //Korrigerar fiendens position när den kolliderar med worldtiles
        foreach (var colTile in collidingTiles)
        {
            if (colTile != null)
                CalculateCollisionSize(direction, colTile, saucer);
        }  
        
        //Om spelaren och fienden kolliderar så är damagetimern aktiv
        if (cave.worldTiles.Where(worldTile => Raylib.CheckCollisionRecs(saucer.entityRect, player.playerRect)).ToList().Count > 0)
            isTimerActive = true;

        //Annars är den inaktiv
        else
        {
            isTimerActive = false;
            damageTimer = 1;
        }

        //En timer som ser till att fienden endast kan skada spelaren 1 gång i sekunden
        if (isTimerActive)
        {
            damageTimer--;
            if (damageTimer <= 0)
            {
                player.Damage(Attack(1));
                damageTimer = 60;
            }
        }
    }

    //Metod som genererar fiender på olika positioner i världen
    public void GenerateEnemies(CaveGeneration cave, Player player)
    {
        for (int x = 0; x < cave.tileGrid.GetLength(0); x++)
        {
            for (int y = 0; y < cave.tileGrid.GetLength(1); y++)
            {
                if (cave.tileGrid[x, y] == 0 && Random.Shared.Next(0, cave.worldSize * cave.worldSize) < 40 && saucers.Count < amountOfSaucers)
                    SpawnEntity(new Saucer(), new Vector2((int)x * cave.worldSize, (int)y * cave.worldSize));
            }
        }
    }

    //Metod som spawnar en fiende på en viss position och lägger till den i en lista
    public void SpawnEntity(Saucer saucer, Vector2 position)
    {
        saucer.position = position;
        saucers.Add(saucer);
    }

    //Metod som ritar ut alla fiender
    public override void Draw()
    {
        for (int i = 0; i < saucers.Count; i++)
            Raylib.DrawRectangleRec(saucers[i].entityRect, Color.BLUE);

    }
}


