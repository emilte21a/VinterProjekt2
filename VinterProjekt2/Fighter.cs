using System.Numerics;
using System.Security.Cryptography;
using Raylib_cs;


public class Fighter
{

    public int hp;

    public Rectangle playerRect;

    public Vector2 playerPosition;

    public Fighter()
    {
        playerRect = new Rectangle(300, 300, 20, 20);
        playerRect.x = playerXmovement(playerRect.x, 4);
        playerRect.y = 400;

    }

    public float playerXmovement(float playerPos, float speed)
    {

        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            playerPos -= speed;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            playerPos += speed;

        return playerPos;
    }


    Vector2 bulletDirection;
    List<Bullet> bullets = new();

    public void Shoot(float speed)
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        Vector2 diff = mousePosition - new Vector2(playerRect.x, playerRect.y);
        bulletDirection = Vector2.Normalize(diff);

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            bullets.Add(new Bullet(new Vector2(playerRect.x + playerRect.width / 2, playerRect.y + playerRect.height / 2), bulletDirection, speed));

        foreach (var bullet in bullets)
        {
            bullet.Update();


        }
        System.Console.WriteLine(bullets.Count);
    }

    public void DrawBullets()
    {
        foreach (var bullet in bullets)
        {
            bullet.Draw();
        }
    }



}

