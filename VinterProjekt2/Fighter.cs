using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using Raylib_cs;


public class Fighter
{
    public int hp;

    public Rectangle playerRect;

    public Vector2 playerPosition;

    private Vector2 lastPos;

    private int playerWidth = 40;

    Texture2D playerSprite = Raylib.LoadTexture("Bilder/CharacterSpriteSheet.png");

    public Camera2D camera { get; init; }

    public Fighter()
    {
        hp = 100;
        playerRect = new Rectangle(0, 0, playerSprite.width / 6, playerSprite.height);
    }

    public float PlayerXmovement(float playerPos, float speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            playerPos -= speed;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            playerPos += speed;

        return playerPos;
    }

    public float PlayerYmovement(float playerPos, float speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            playerPos -= speed;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            playerPos += speed;

        return playerPos;
    }

    Vector2 bulletDirection;
    List<Bullet> bullets = new();

    List<Bullet> bulletsToDestroy = new();

    public void Shoot(float speed)
    {
        Vector2 mousePosition = Raylib.GetMousePosition() - camera.offset;
        Vector2 pos = new Vector2(playerRect.x, playerRect.y);
        Vector2 diff = mousePosition - pos;
        bulletDirection = Vector2.Normalize(diff + pos);

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            bullets.Add(new Bullet(new Vector2(playerRect.x + playerRect.width / 2, playerRect.y + playerRect.height / 2), bulletDirection, speed));

        foreach (var bullet in bullets)
        {
            bullet.Update();
            if (bullet.ShouldDestroy(playerRect))
            {
                bulletsToDestroy.Add(bullet);
            }
        }
    }

    public void DrawBullets()
    {
        foreach (var bullet in bullets)
            bullet.Draw();
    }
    int frame = 0;
    int timer = 0;


    public float DrawPlayer()
    {
        timer++;
        int maxFrames = 6;
        if (timer > 10)
        {

            frame++;
            timer = 0;
        }
        return frame %= maxFrames;
    }

    public bool CheckCollision(CaveGeneration cave)
    {
        return cave.worldTiles.Any(worldTile => Raylib.CheckCollisionRecs(playerRect, worldTile.tileRect));
    }

    public void ResetPosition(CaveGeneration cave)
    {
        lastPos = new Vector2(playerRect.x, playerRect.y);


        if (CheckCollision(cave))
        {
            var collidingTile = cave.worldTiles.FirstOrDefault(worldTile => Raylib.CheckCollisionRecs(playerRect, worldTile.tileRect));

            if (collidingTile != null)
            {
                float intersectionWidth = Math.Min(playerRect.x + playerRect.width, collidingTile.tileRect.x + collidingTile.tileRect.width) - Math.Max(playerRect.x, collidingTile.tileRect.x);
            }
            // playerRect.x = lastPos.X;
            // playerRect.y = lastPos.Y;
        }
    }
}
