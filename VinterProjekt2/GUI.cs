

using Raylib_cs;

public class GUI
{

    private Texture2D HPtexture = Raylib.LoadTexture("Bilder/HP.png");




    public void DrawGUI(Player _player, Saucer _saucer)
    {
        Raylib.DrawText($"{_player.playerPosition}", 20, 40, 20, Color.LIME);
        Raylib.DrawText($"{_saucer.position}", 20, 60, 20, Color.LIME);

        for (int i = 0; i < _player.hp; i++)
            Raylib.DrawTexture(HPtexture, i * 70 + 800, 50, Color.WHITE);
        
        
        Raylib.DrawFPS(20, 20);
    }

}