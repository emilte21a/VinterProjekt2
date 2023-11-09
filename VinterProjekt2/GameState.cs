using Raylib_cs;
using System.Numerics;
public class GameState
{
    const int screenWidth = 720;
    const int screenHeight = 720;
    Fighter player;
    CaveGeneration caveGeneration;

    Camera2D camera;
    public GameState()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "scary");
        Raylib.SetTargetFPS(60);

        player = new Fighter();
        caveGeneration = new CaveGeneration();
        caveGeneration.GenerateTerrain();
        camera = new();
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


    private void Update()
    {
        player.playerRect.x = player.playerXmovement(player.playerRect.x, 10);
        CameraUpdate();
        camera.target = player.playerPosition;
        camera.offset = new Vector2(screenWidth/2,screenHeight/2);
        
        player.Shoot(15);
    }

    private void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        
        caveGeneration.Draw();
        Raylib.DrawRectangleRec(player.playerRect, Color.ORANGE);
        player.DrawBullets();


        Raylib.EndDrawing();
    }

    private void CameraUpdate(){
        player.playerPosition.X = player.playerRect.x;
        player.playerPosition.Y = player.playerRect.y;
    }
}
