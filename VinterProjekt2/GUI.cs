

using Raylib_cs;

public class GUI
{
    Texture2D HPtexture = Raylib.LoadTexture("Bilder/HP.png");

    //Metod som ritar ut det grafiska användargränssnittet när spelaren spelar spelet
    public void DrawGUI(Player _player, Saucer _saucer)
    {
        if (_player.hp == 1)
            DrawLowHpWarning();

        Raylib.DrawText($"Points: {_player.amountOfPoints}", 20, 50, 60, Color.LIME);

        for (int i = 0; i < _player.hp; i++)
            Raylib.DrawTexture(HPtexture, i * 70 + 800, 50, Color.WHITE);


        Raylib.DrawFPS(25, 25);
    }

    Color red = new Color(255, 0, 0, 90);

    //Metod som ger visuell varning när spelaren har ett liv kvar i form av ett blinkande rött ljus som täcker hela skärmen
    private void DrawLowHpWarning()
    {
        if (red.a > 0)
            red.a -= (byte)1.3f;

        else if (red.a <= 0)
            red.a = 110;

        Raylib.DrawRectangle(0, 0, GameState.screenWidth, GameState.screenHeight, red);
    }

}