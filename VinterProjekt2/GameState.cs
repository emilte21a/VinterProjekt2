using Raylib_cs;
using System.Numerics;
public class GameState
{
    const int screenWidth = 720;
    const int screenHeight = 720;
    Fighter player;
    CaveGeneration caveGeneration;

    Camera2D camera;
    public GameState() //Fungerar som en start funktion i unity.
    {
        Raylib.InitWindow(screenWidth, screenHeight, "scary");
        Raylib.SetTargetFPS(60);

       // caveGeneration = new CaveGeneration();
       // caveGeneration.GenerateTerrain();

        camera = new()
        {
            zoom = 0.5f,
            offset = new Vector2(screenWidth / 2, screenHeight / 2)
            // target = player.playerPosition
        };
        player = new Fighter() {camera = camera};
        camera.target = player.playerPosition;
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

        Raylib.EndMode2D();

        Raylib.DrawFPS(20, 20);

        Raylib.EndDrawing();
    }

    private void CameraUpdate()
    {
        player.playerPosition.X = player.playerRect.x;
        player.playerPosition.Y = player.playerRect.y;
    }
}
