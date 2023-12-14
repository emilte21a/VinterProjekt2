

using Raylib_cs;

public class GUI
{

    private Texture2D HPtexture = Raylib.LoadTexture("Bilder/HP.png");




    public void DrawGUI(Player _player, Saucer _saucer)
    {
        Raylib.DrawText($"Points: {_player.points}", 20, 50, 60, Color.LIME);

        for (int i = 0; i < _player.hp; i++)
            Raylib.DrawTexture(HPtexture, i * 70 + 800, 50, Color.WHITE);
        
        
        Raylib.DrawFPS(25, 25);
    }

}