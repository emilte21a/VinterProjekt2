using Raylib_cs;
using System.Numerics;
public class GameState
{

    const int screenWidth = 1080;
    const int screenHeight = 1080;


    //Instanser
    Player player;
    Saucer saucer;
    CaveGeneration caveGeneration;
    StateManager currentState;
    UniversalMath uMath;
    GUI gUI;
    Collectible collectible;

    Texture2D cursorSprite;
    Texture2D backgroundTexture;
    Texture2D coreTexture;
    Camera2D camera;
    public GameState() //Fungerar som en start funktion i unity.
    {
        Raylib.InitWindow(screenWidth, screenHeight, "scary");

        cursorSprite = Raylib.LoadTexture("Bilder/MouseCursor.png");
        backgroundTexture = Raylib.LoadTexture("Bilder/Bakgrund.png");
        coreTexture = Raylib.LoadTexture("Bilder/Core.png");
        Raylib.SetTargetFPS(60);
        currentState = StateManager.Start;

        caveGeneration = new();
        uMath = new();
        gUI = new();
        collectible = new();


        camera = new()
        {
            target = new Vector2(0, 0),
            zoom = 0.8f,
            offset = new Vector2(screenWidth / 2, screenHeight / 2)
        };
        player = new Player() { camera = camera };

        saucer = new Saucer();
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
        switch (currentState)
        {
            case StateManager.Start:
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_F))
                {
                    currentState = StateManager.Game;
                    caveGeneration.GenerateTerrain();
                    collectible.GenerateCollectibles(caveGeneration);
                    saucer.GenerateEnemies(caveGeneration);
                    Raylib.HideCursor();
                    Raylib.DisableCursor();
                }
                break;

            case StateManager.Game:
                player.ControlPlayerPosition(caveGeneration, 10);
                CameraUpdate();
                saucer.Update(player.playerPosition, caveGeneration, player);
                EnableCursor();
                player.Shoot(15, caveGeneration);
                break;
        }
    }

    private void Draw() //Ritar ut spelet
    {
        Raylib.BeginDrawing();

        switch (currentState)
        {
            case StateManager.Start:
                Raylib.DrawText("scary game", screenWidth / 2, screenHeight / 2, 50, Color.ORANGE);
                break;

            case StateManager.Game:
                Raylib.ClearBackground(Color.WHITE);
                Raylib.DrawTexture(backgroundTexture, 0, 0, Color.WHITE);
                Raylib.BeginMode2D(camera);
                caveGeneration.Draw();
                collectible.DrawCollectibles();
                //Raylib.DrawTexture(coreTexture, caveGeneration.worldSize * 100 / 2 - coreTexture.width / 2, caveGeneration.worldSize * 100 / 2 - coreTexture.height / 2, Color.WHITE);
                saucer.Draw();
                Raylib.DrawTextureRec(player.playerSprite, new Rectangle(player.DrawPlayer(6, 0.3f) * player.playerSprite.width / 6, 0, player.playerSprite.width / 6, player.playerSprite.height), player.playerPosition - new Vector2(player.playerRect.width / 2, player.playerRect.height / 2), Color.WHITE);

                player.DrawBullets();

                Raylib.DrawTexturePro(cursorSprite, new Rectangle(0, 0, cursorSprite.width, cursorSprite.height), new Rectangle((int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).X, (int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).Y, cursorSprite.width, cursorSprite.height), new Vector2((int)cursorSprite.width / 2, (int)cursorSprite.height / 2), (int)RadiansToDegrees(), Color.WHITE);
                Raylib.EndMode2D();
                gUI.DrawGUI(player, saucer);
                break;
        }
        Raylib.EndDrawing();

    }
    private void CameraUpdate()
    {
        player.playerPosition.X = player.playerRect.x + player.playerRect.width / 2;
        player.playerPosition.Y = player.playerRect.y + player.playerRect.height / 2;
        camera.target = uMath.Lerp(camera.target, player.playerPosition, 0.1f);
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

    private void EnableCursor()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_T) && Raylib.IsCursorHidden())
        {
            Raylib.ShowCursor();
            Raylib.EnableCursor();
        }

        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_T) && !Raylib.IsCursorHidden())
        {
            Raylib.HideCursor();
            Raylib.DisableCursor();
        }
    }
}
