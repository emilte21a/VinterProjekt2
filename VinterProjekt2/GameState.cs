using Raylib_cs;
using System.Numerics;
public class GameState
{
    const int screenWidth = 720;
    const int screenHeight = 720;
    Fighter player;
    CaveGeneration caveGeneration;

    Texture2D cursor;


    Camera2D camera;
    public GameState() //Fungerar som en start funktion i unity.
    {
        Raylib.InitWindow(screenWidth, screenHeight, "scary");
        cursor = Raylib.LoadTexture("Bilder/MouseCursor.png");
        Raylib.SetTargetFPS(60);

        //caveGeneration = new CaveGeneration();
        //caveGeneration.GenerateTerrain();

        camera = new()
        {
            zoom = 0.5f,
            offset = new Vector2(screenWidth / 2, screenHeight / 2)
            // target = player.playerPosition
        };
        player = new Fighter() { camera = camera };

        Raylib.HideCursor();
        Raylib.DisableCursor();
    }
    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();
            Draw();
        }

        Raylib.CloseWindow();
    }


    private void Update() //Uppdaterar logiken i spelet
    {
        player.playerRect.x = player.PlayerXmovement(player.playerRect.x, 10);
        player.playerRect.y = player.PlayerYmovement(player.playerRect.y, 10);
        CameraUpdate();
        camera.target = player.playerPosition;

        player.Shoot(15);
    }



    private void Draw() //Ritar ut spelet
    {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(camera);
        Raylib.ClearBackground(Color.WHITE);

        //caveGeneration.Draw();
        Raylib.DrawRectangleRec(player.playerRect, Color.ORANGE);
        player.DrawBullets();

        Raylib.DrawTextureEx(cursor, new Vector2((int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).X + (cursor.width / Raylib.GetMousePosition().X)/2, (int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).Y + (cursor.height / Raylib.GetMousePosition().Y)/2), (float)RadiansToDegrees() + 90, 1, Color.WHITE);
        Raylib.DrawRectangle((int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).X, (int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).Y, 5, 5, Color.BLUE);
        System.Console.WriteLine(RadiansToDegrees());
        Raylib.EndMode2D();

        Raylib.DrawFPS(20, 20);

        Raylib.EndDrawing();
    }

    private void CameraUpdate()
    {
        player.playerPosition.X = player.playerRect.x + player.playerRect.width / 2;
        player.playerPosition.Y = player.playerRect.y + player.playerRect.height / 2;
    }

    private double RadiansToDegrees()
    {
        Vector2 mousePosition = Raylib.GetMousePosition() - camera.offset;
        Vector2 pos = new Vector2(player.playerRect.x, player.playerRect.y);
        Vector2 diff = mousePosition - pos;
        Vector2 cursorDirection = Vector2.Normalize(diff + pos);
        double radians = Math.Atan2(cursorDirection.Y, cursorDirection.X);

        return (radians * (180 / Math.PI));
    }
}
