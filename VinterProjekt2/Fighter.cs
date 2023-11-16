using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using Raylib_cs;


public class Fighter
{

    public int hp;

    public Rectangle playerRect;

    public Vector2 playerPosition;

    private int playerWidth = 40;

    public Camera2D camera { get; init; }

    public Fighter()
    {
        hp = 100;
        playerRect = new Rectangle(0, 0, playerWidth, playerWidth);
    }

    public float PlayerXmovement(float playerPos, float speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            playerPos -= speed;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            playerPos += speed;

        return playerPos;
    }

    public float PlayerYmovement(float playerPos, float speed){
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            playerPos-=speed;
        
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            playerPos+=speed;

        return playerPos;
    }

    Vector2 bulletDirection;
    List<Bullet> bullets = new();

    public void Shoot(float speed)
    {
        Vector2 mousePosition = Raylib.GetMousePosition() - camera.offset;
        Vector2 pos = new Vector2(playerRect.x, playerRect.y);
        Vector2 diff = mousePosition - pos;
        bulletDirection = Vector2.Normalize(diff+pos);    
        
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            bullets.Add(new Bullet(new Vector2(playerRect.x + playerRect.width / 2, playerRect.y + playerRect.height / 2), bulletDirection, speed));

        foreach (var bullet in bullets)
            bullet.Update();
        
        //System.Console.WriteLine(bullets.Count);
    }

    public void DrawBullets()
    {
        foreach (var bullet in bullets)
            bullet.Draw();
        
    }
}

