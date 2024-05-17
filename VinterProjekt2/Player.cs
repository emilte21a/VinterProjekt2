using System.Dynamic;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using Microsoft.VisualBasic;
using Raylib_cs;

public class Player
{
    //Spelarens liv
    public int hp;

    //Mängden collectibles som spelaren har plockat upp
    public float amountOfPoints = 0;

    //Spelar rektangeln
    public Rectangle playerRect;

    //Spelarens position i en vector2
    public Vector2 playerPosition;

    //Spelar spriten
    public Texture2D playerSprite = Raylib.LoadTexture("Bilder/CharacterSpriteSheet.png");

    //"Spelarens kamera"
    public Camera2D camera { get; init; }

    public Player()
    {
        hp = 4;
        playerRect = new Rectangle(-50, 0, playerSprite.width / 6, playerSprite.height);
    }

    //Metod som ändrar spelarens x-position
    public float MovePlayerX(float playerPosX, float speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            playerPosX -= speed * Raylib.GetFrameTime() * 50;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            playerPosX += speed * Raylib.GetFrameTime() * 50;
        }

        return playerPosX;
    }

    //Metod som ändrar spelarens y-position
    public float MovePlayerY(float playerPosY, float speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        {
            playerPosY -= speed * Raylib.GetFrameTime() * 50;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        {
            playerPosY += speed * Raylib.GetFrameTime() * 50;
        }

        return playerPosY;
    }

    //En enum för alla olika riktningar som spelaren rör sig
    public enum PlayerDirection
    {
        Idle,
        Left,
        Right,
        Up,
        Down,
        RightUp,
        RightDown,
        LeftUp,
        LeftDown
    }

    //Metod som returnerar spelarens riktning beroende på vilka knappar som trycks
    public PlayerDirection GetPlayerDirection()
    {
        bool isKeyDownA = Raylib.IsKeyDown(KeyboardKey.KEY_A);
        bool isKeyDownD = Raylib.IsKeyDown(KeyboardKey.KEY_D);
        bool isKeyDownW = Raylib.IsKeyDown(KeyboardKey.KEY_W);
        bool isKeyDownS = Raylib.IsKeyDown(KeyboardKey.KEY_S);

        if (isKeyDownA && !isKeyDownW && !isKeyDownS && !isKeyDownD)
            return PlayerDirection.Left;

        else if (isKeyDownD && !isKeyDownW && !isKeyDownS && !isKeyDownA)
            return PlayerDirection.Right;

        else if ((!isKeyDownS && !isKeyDownA && !isKeyDownD && isKeyDownW) || (!isKeyDownS && isKeyDownA && isKeyDownD && isKeyDownW))
            return PlayerDirection.Up;

        else if ((isKeyDownS && !isKeyDownA && !isKeyDownD && !isKeyDownW) || (isKeyDownS && isKeyDownA && isKeyDownD && !isKeyDownW))
            return PlayerDirection.Down;

        else if (isKeyDownW && !isKeyDownA && isKeyDownD && !isKeyDownS)
            return PlayerDirection.RightUp;

        else if (isKeyDownS && !isKeyDownA && isKeyDownD && !isKeyDownW)
            return PlayerDirection.RightDown;

        else if (isKeyDownW && isKeyDownA && !isKeyDownD && !isKeyDownS)
            return PlayerDirection.LeftUp;

        else if (isKeyDownS && isKeyDownA && !isKeyDownD && !isKeyDownW)
            return PlayerDirection.LeftDown;

        else
            return PlayerDirection.Idle;
    }

    Stack<Bullet> bullets = new(); //Lista med alla skott som är aktiva
    List<Bullet> bulletsToDestroy = new(); //Lista med de skott som ska förstöras

    //Metod som tillåter spelaren att skjuta skott
    public void Shoot(float speed, CaveGeneration cave)
    {
        Vector2 mousePosition = Raylib.GetMousePosition() - camera.offset;
        Vector2 pos = new Vector2(playerRect.x, playerRect.y);
        Vector2 diff = mousePosition - pos;
        Vector2 bulletDirection = Vector2.Normalize(diff + pos);

        //Hämta vilken riktning som skottet ska skjutas mot

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            bullets.Push(new Bullet(new Vector2(playerRect.x + playerRect.width / 2, playerRect.y + playerRect.height / 2), bulletDirection, speed));

        //Om vänster musknapp trycks
        //Skapa ett nytt skott från spelarens position mot musens riktning

        foreach (var bullet in bullets.ToList())
        {
            bullet.Update();

            if (bullet.ShouldDestroy(playerRect, cave))
            {
                bulletsToDestroy.Add(bullet);
            }
        }
        //Uppdatera varje skotts position och om de ska förstöras

        foreach (var bullet in bulletsToDestroy)
        {
            bullets.Pop();
        }

        bulletsToDestroy.Clear();
    }

    //Metod som ritar ut varje skott
    public void DrawBullets()
    {
        foreach (var bullet in bullets)
            bullet.Draw();
    }


    int frame = 1;
    float elapsed = 0;

    //Metod som bestämmer vilken frame en animation ska vara på från ett spritesheet
    public int DrawPlayer(int maxFrames, float timePerFrame)
    {
        elapsed += Raylib.GetFrameTime();
        if (elapsed > timePerFrame)
        {
            frame++;
            elapsed = 0;
        }
        return frame %= maxFrames;
    }

    //Metod som hanterar hur mycket skada spelaren tar
    public void Damage(int _damage)
    {
        hp -= _damage;
    }

    //[Kollisioner]==========================================================================

    //Metod som korrigerar spelarens position när de kolliderar med worldtiles beroende på dess riktning 
    public void CalculateCollisionSize(PlayerDirection direction, Tile collidingTile)
    {
        var collisionRec = Raylib.GetCollisionRec(playerRect, collidingTile.tileRect);

        switch (direction)
        {
            case PlayerDirection.Right:
                playerRect.x -= (int)collisionRec.width;

                break;

            case PlayerDirection.Left:
                playerRect.x += (int)collisionRec.width;
                break;

            case PlayerDirection.Down:
                playerRect.y -= (int)collisionRec.height;
                break;

            case PlayerDirection.Up:
                playerRect.y += (int)collisionRec.height;
                break;

            case PlayerDirection.RightUp:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y += (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x -= (int)collisionRec.width;

                break;

            case PlayerDirection.RightDown:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y -= (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x -= (int)collisionRec.width;

                break;

            case PlayerDirection.LeftUp:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y += (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x += (int)collisionRec.width;

                break;

            case PlayerDirection.LeftDown:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y -= (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x += (int)collisionRec.width;

                break;

            case PlayerDirection.Idle:


                break;
        }
    }

    public List<Tile> collidingTiles; //En lista med de worldtiles som spelaren kolliderar med

    //Metod som returnerar en lista med de worldtiles som spelaren kolliderar med
    public List<Tile> CheckCollisionsWithTiles(CaveGeneration cave)
    {
        return cave.worldTiles.Where(worldTile => Raylib.CheckCollisionRecs(playerRect, worldTile.tileRect)).ToList(); //Returnerar en lista med de Tiles som spelaren kolliderar med
    }

    //Metod som returnerar en lista med de collectibles som spelaren kolliderar med
    public List<Collectible> CheckCollisionsWithCollectibles(Collectible collectible)
    {
        List<Collectible> collisionWithCollectible = new List<Collectible>();

        foreach (var kvp in collectible.collectibles)
        {
            if (Raylib.CheckCollisionRecs(playerRect, kvp.Value.collectibleRec))
            {
                collisionWithCollectible.Add(kvp.Value);
            }
        }
        return collisionWithCollectible;//collectible.collectibles.Where(collectible => Raylib.CheckCollisionRecs(playerRect, collectible.Value.collectibleRec)).ToList(); //Returnerar en lista med de Tiles som spelaren kolliderar med
    }

    //Metod som uppdaterar spelarens logik
    public void Update(CaveGeneration cave, float movementSpeed, Collectible collectible)
    {
        collectible.collectiblesToDestroy = CheckCollisionsWithCollectibles(collectible); //Lista med de collectibles som ska tas bort

        foreach (var collectedItem in collectible.collectiblesToDestroy)
        {
            collectible.collectibles.Remove(1);
            amountOfPoints += 1;
        }
        //För varje collectible som spelaren plockat upp, så ska spelarens poäng öka med 1 samtidigt som det föremålet som plockats upp förstörs

        playerRect.x = (int)MovePlayerX(playerRect.x, movementSpeed);
        playerRect.y = (int)MovePlayerY(playerRect.y, movementSpeed);

        collidingTiles = CheckCollisionsWithTiles(cave);

        PlayerDirection direction = GetPlayerDirection();

        foreach (var colTile in collidingTiles)
        {
            if (colTile != null)
            {
                CalculateCollisionSize(direction, colTile);
            }
        }

        Shoot(15, cave); //Uppdatera skjutmetoden
    }
}
