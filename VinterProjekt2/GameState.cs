using Raylib_cs;
using System.Numerics;
public class GameState
{

    const int screenWidth = 850;
    const int screenHeight = 850;
    Player player;

    //Instanser
    CaveGeneration caveGeneration;
    StateManager currentState;
    CameraSmooth cameraSmoothing;

    Texture2D cursorSprite;
    Camera2D camera;
    public GameState() //Fungerar som en start funktion i unity.
    {
        Raylib.InitWindow(screenWidth, screenHeight, "scary");

        cursorSprite = Raylib.LoadTexture("Bilder/MouseCursor.png");
        Raylib.SetTargetFPS(60);
        currentState = StateManager.Start;

        caveGeneration = new();
        cameraSmoothing = new();

        camera = new()
        {
            target = new Vector2(0, 0),
            zoom = 0.6f,
            offset = new Vector2(screenWidth / 2, screenHeight / 2)
        };
        player = new Player() { camera = camera };
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
        if (currentState == StateManager.Start)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F))
            {
                currentState = StateManager.Game;
                caveGeneration.GenerateTerrain();
                Raylib.HideCursor();
                Raylib.DisableCursor();
            }
        }

        else if (currentState == StateManager.Game)
        {
            player.ControlPlayerPosition(caveGeneration, 10);

            CameraUpdate();

            camera.target = cameraSmoothing.Lerp(camera.target, player.playerPosition, 0.1f);
            EnableCursor();

            player.Shoot(15);
        }
        else if (currentState == StateManager.GameOver)
        {

        }
    }

    private void Draw() //Ritar ut spelet
    {
        Raylib.BeginDrawing();
        if (currentState == StateManager.Start)
        {
            Raylib.DrawText("scary game", screenWidth / 2, screenHeight / 2, 50, Color.ORANGE);
        }

        else if (currentState == StateManager.Game)
        {
            Rectangle sourceRect = new Rectangle(player.DrawPlayer(6, 10) * player.playerSprite.width / 6, 0, player.playerSprite.width / 6, player.playerSprite.height);
            Raylib.BeginMode2D(camera);
            Raylib.ClearBackground(Color.WHITE);

            caveGeneration.Draw();

            Raylib.DrawRectangleRec(player.playerRect, Color.ORANGE);


            // Raylib.DrawTextureRec(playerSprite, sourceRect, player.playerPosition - new Vector2(player.playerRect.width / 2, player.playerRect.height / 2), Color.WHITE);

            player.DrawBullets();

            Raylib.DrawTexturePro(cursorSprite, new Rectangle(0, 0, cursorSprite.width, cursorSprite.height), new Rectangle((int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).X, (int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).Y, cursorSprite.width, cursorSprite.height), new Vector2((int)cursorSprite.width / 2, (int)cursorSprite.height / 2), (int)RadiansToDegrees(), Color.WHITE);
            Raylib.EndMode2D();

            Raylib.DrawFPS(20, 20);

        }
        else if (currentState == StateManager.GameOver)
        {

        }
        Raylib.EndDrawing();

    }
    Vector2 lastPosition;
    private void CameraUpdate()
    {
        lastPosition = player.playerPosition;
        player.playerPosition.X = player.playerRect.x + player.playerRect.width / 2;
        player.playerPosition.Y = player.playerRect.y + player.playerRect.height / 2;
    }

    private double RadiansToDegrees()
    {
        Vector2 mousePosition = Raylib.GetMousePosition() - camera.offset;
        Vector2 pos = new Vector2(player.playerRect.x + player.playerSprite.width / 6, player.playerRect.y + player.playerSprite.height / 2);
        Vector2 diff = mousePosition - pos;
        Vector2 cursorDirection = Vector2.Normalize(diff + pos);
        double radians = Math.Atan2(cursorDirection.Y, cursorDirection.X);

        return (radians * (180 / Math.PI));
    }

    bool cursorIsShown = false;

    private void EnableCursor()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_T) && !cursorIsShown)
        {
            Raylib.ShowCursor();
            Raylib.EnableCursor();
            cursorIsShown = true;
        }

        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_T) && cursorIsShown)
        {
            Raylib.HideCursor();
            Raylib.EnableCursor();
            cursorIsShown = false;
        }
    }
}
