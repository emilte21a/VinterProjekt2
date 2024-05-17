using Raylib_cs;
using System.Numerics;
public class GameState
{

    public static int screenWidth = 1080;
    public static int screenHeight = 1080;


    //Instanser av klasser
    Player player;
    Saucer saucer;
    CaveGeneration caveGeneration;
    SceneManager currentScene;
    UniversalMath uMath;
    GUI gUI;
    Collectible collectible;

    //Texturer
    Texture2D cursorSprite;
    Texture2D backgroundTexture;

    //2D kamera
    Camera2D camera;

    //Mängden poäng som behövs för att vinna spelet
    private int _pointsToWin = 10;

    //Vad tiden var när spelaren startade spelet
    int timeAtStart;

    //Vad tiden var när spelaren klarade eller förlorade spelet
    int timeAtFinish;

    public GameState() //Fungerar som en start funktion i unity.
    {
        Raylib.InitWindow(screenWidth, screenHeight, "scary");
        Raylib.SetTargetFPS(60);

        //Instansera de texturer som används
        cursorSprite = Raylib.LoadTexture("Bilder/MouseCursor.png");
        backgroundTexture = Raylib.LoadTexture("Bilder/Bakgrund.png");
        
        //Spelet börjar i start screen.
        currentScene = SceneManager.Start;

        //Insansera klassinstanserna
        caveGeneration = new();
        uMath = new();
        gUI = new();
        collectible = new();

        //Skapa en ny kamera med specifikationer
        camera = new()
        {
            target = new Vector2(0, 0),
            zoom = 0.8f,
            offset = new Vector2(screenWidth / 2, screenHeight / 2)
        };
        player = new Player() { camera = camera };
        //Kamerans target är spelaren

        saucer = new Saucer();
    }
    
    //Metod som startar och avslutar spelet
    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();
            Draw();
        }

        Raylib.CloseWindow();
    }

    //Metod som uppdaterar logiken i spelet
    private void Update() 
    {
        //Switch statement för currentScene
        switch (currentScene)
        {
            case SceneManager.Start:
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    currentScene = SceneManager.Game; 
                    caveGeneration.GenerateTerrain();
                    collectible.GenerateCollectibles(caveGeneration);
                    saucer.GenerateEnemies(caveGeneration, player);
                    Raylib.HideCursor();
                    Raylib.DisableCursor();
                    timeAtStart = (int)Raylib.GetTime();
                }
                break;
                //Om spelaren trycker på ENTER
                    //Gör currentScene till Game
                    //Generera terräng
                    //Generera collectibles
                    //Generera fiender
                    //Göm och stäng av muspekaren
                    //Spara tiden som spelaren startade spelet i en int

            case SceneManager.Game:
                player.Update(caveGeneration, 10, collectible);
                saucer.Update(player.playerPosition, caveGeneration, player);
                CameraUpdate();
                ShowCursor();
                if (player.amountOfPoints == _pointsToWin || player.hp == 0)
                {
                    currentScene = SceneManager.GameOver;
                    timeAtFinish = (int)Raylib.GetTime();
                }
                break;
            //Uppdatera spelaren
            //Uppdatera fienderna
            //Uppdatera kameran
            //Uppdatera varken muspekaren ska visas eller inte
            //Om spelaren har samlat mängden poäng som behövs för att vinna eller om spelaren inte har något liv kvar
                //Gör currentScene till GameOver
                //Spara tiden som spelaren avslutade spelet i en int
        }
    }
    int parallaxScroll = 0;

    //Metod för att rita ut spelets grafik
    private void Draw() 
    {
        Raylib.BeginDrawing();

        switch (currentScene)
        {
            case SceneManager.Start:
                parallaxScroll++;
                Raylib.DrawTextureRec(backgroundTexture, new Rectangle(parallaxScroll * 0.2f, parallaxScroll * 0.7f, backgroundTexture.width, backgroundTexture.height), new Vector2(0, 0), Color.WHITE);
                Raylib.DrawText("Evader", 40, 40, 50, Color.ORANGE);
                Raylib.DrawText("Instructions", 40, 150, 30, Color.WHITE);
                Raylib.DrawText("- Shoot blocks to break them.", 40, 200, 25, Color.WHITE);
                Raylib.DrawText("- Evade the saucers patrolling the moon!", 40, 250, 25, Color.WHITE);
                Raylib.DrawText("- Gather 10 points by picking up the collectibles scattered throughout the map.", 40, 300, 25, Color.WHITE);
                Raylib.DrawText("Press ENTER to start!", screenWidth / 4, screenHeight / 2, 50, Color.WHITE);
                break;

            case SceneManager.Game:
                Raylib.ClearBackground(Color.WHITE);
                Raylib.DrawTexture(backgroundTexture, 0, 0, Color.WHITE);
                Raylib.BeginMode2D(camera);
                caveGeneration.Draw();
                collectible.DrawCollectibles();
                saucer.Draw();
                Raylib.DrawTextureRec(player.playerSprite, new Rectangle(player.DrawPlayer(6, 0.3f) * player.playerSprite.width / 6, 0, player.playerSprite.width / 6, player.playerSprite.height), player.playerPosition - new Vector2(player.playerRect.width / 2, player.playerRect.height / 2), Color.WHITE);

                player.DrawBullets();

                Raylib.DrawTexturePro(cursorSprite, new Rectangle(0, 0, cursorSprite.width, cursorSprite.height), new Rectangle((int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).X, (int)(Raylib.GetMousePosition() - camera.offset + player.playerPosition).Y, cursorSprite.width, cursorSprite.height), new Vector2((int)cursorSprite.width / 2, (int)cursorSprite.height / 2), (int)RadiansToDegrees(), Color.WHITE);
                Raylib.EndMode2D();

                gUI.DrawGUI(player, saucer);
                break;

            case SceneManager.GameOver:
                Raylib.ClearBackground(Color.BLACK);
                if (player.amountOfPoints == _pointsToWin)
                {
                    Raylib.DrawText("You won!", screenWidth / 2, screenHeight / 2, 60, Color.BLUE);
                    Raylib.DrawText($"You managed to escape the moon in {(timeAtFinish - timeAtStart)} seconds!", screenWidth / 4, 600, 20, Color.BLUE);
                }
                else if (player.hp == 0)
                {
                    Raylib.DrawText("You lost..", screenWidth / 2, screenHeight / 2, 60, Color.RED);
                    Raylib.DrawText("Try to avoid touching the enemy!", screenWidth / 5, screenHeight / 2 + 100, 40, Color.RED);
                }
                break;
            //Olika gameover skärmar beroende på om spelaren är död eller vann
        }

        Raylib.EndDrawing();

    }

    //Metod som sparar spelar rektangelns position i en Vector2 och lerpar (linear interpolation) för att uppdatera kamerans position
    private void CameraUpdate()
    {
        player.playerPosition.X = player.playerRect.x + player.playerRect.width / 2;
        player.playerPosition.Y = player.playerRect.y + player.playerRect.height / 2;
        camera.target = uMath.Lerp(camera.target, player.playerPosition + Vector2.Normalize(Raylib.GetMousePosition()), 0.1f);
    }

    //Metod som konverterar radians till grader för att få rätt rotation för muspekar spriten
    private double RadiansToDegrees()
    {
        Vector2 mousePosition = Raylib.GetMousePosition() - camera.offset;
        Vector2 pos = new Vector2(player.playerRect.x + player.playerSprite.width / 6, player.playerRect.y + player.playerSprite.height / 2);
        Vector2 diff = mousePosition - pos;
        Vector2 cursorDirection = Vector2.Normalize(diff + pos);
        double radians = Math.Atan2(cursorDirection.Y, cursorDirection.X);

        return (radians * (180 / Math.PI));
    }

    //Metod som kollar om muspekaren ska vara aktiv eller inte
    private void ShowCursor()
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
