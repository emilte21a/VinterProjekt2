using Raylib_cs;
using System.Numerics;
public class CaveGeneration
{
    float surfaceValue = 146f; //Desto högre talet är, desto mindre block genereras

    public readonly int worldSize = 100; //Bredd och höjd 

    private int tileSize = 100; //Storleken för varje tile

    private int Seed; 

    Image noiseImage; //En bild som sparas i processorn

    Color v;

    private Texture2D perlinImage;
    private Texture2D stoneWallpaper;

    public List<Tile> worldTiles = new(); //En lista med alla tiles som genererats
    private List<Vector2> backgroundTiles = new(); //En lista med bakgrundstiles 

    public int[,] tileGrid; //En 2D array

    public CaveGeneration()
    {
        Seed = Random.Shared.Next(-1000, 1000); //Seed är ett slumpmässigt tal vilket innebär att spelet kommer se annorlunda ut varje gång man startar det
        stoneWallpaper = Raylib.LoadTexture("Bilder/StoneWallpaper.png");
        tileGrid = new int[worldSize, worldSize]; //Instansera 2D arrayen med bredden och höjden worldsize
    }

    //Metod som genererar terrängen i världen
    public void GenerateTerrain()
    {
        noiseImage = Raylib.GenImagePerlinNoise(worldSize, worldSize, Seed, Seed, 10); 
        perlinImage = Raylib.LoadTextureFromImage(noiseImage);

        //Ladda in en perlinnoise bild med bredden och höjden worldsize på processorn
        //Ladda in perlinnoise bilden i en texture istället för en image

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                v = Raylib.GetImageColor(noiseImage, x, y);
                if (v.r > surfaceValue)
                    PlaceTile(new Stone(), new Vector2((int)x * tileSize, (int)y * tileSize));

                else if (v.r < surfaceValue && v.r > 80)
                    PlaceBackground(new Vector2((int)x * tileSize, (int)y * tileSize));

                //För varje x och y värde mindre än worldsize så är v färgen på just den pixeln av noiseimage
                    //Om det röda värdet på V är större än surfacevalue så ska en tile placeras ut på det x och y värdet
            }
        }

        //Ladda ur perlinnoise bilden från processorn
        Raylib.UnloadImage(noiseImage);
    }

    //Metod som placerar ut en tile på respektive position och ändrar värdet på den positionen till 1 i tileGrid
    private void PlaceTile(Tile tile, Vector2 position)
    {
        tile.Pos = position;
        worldTiles.Add(tile);
        tileGrid[(int)position.X / tileSize, (int)position.Y / tileSize] = 1;
    }

    //Metod som lägger till en bakgrunds tile
    private void PlaceBackground(Vector2 position)
    {
        backgroundTiles.Add(position);
    }

    //Metod som returnerar en lista med de tiles som ska förstöras
    public List<Tile> TileToDestroy(CaveGeneration cave, Rectangle bulletRec)
    {
        return cave.worldTiles.Where(worldTile => Raylib.CheckCollisionRecs(bulletRec, worldTile.tileRect)).ToList();
    }

    //Metod som ritar ut tiles och bakgrundtiles
    public void Draw()
    {
        for (int i = 0; i < worldTiles.Count; i++)
            Raylib.DrawTexture(worldTiles[i].texture, (int)worldTiles[i].Pos.X, (int)worldTiles[i].Pos.Y, Color.WHITE);

        for (int i = 0; i < backgroundTiles.Count; i++)
            Raylib.DrawTexture(stoneWallpaper, (int)backgroundTiles[i].X, (int)backgroundTiles[i].Y, Color.WHITE);

    }
}