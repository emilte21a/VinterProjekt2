using System.Numerics;
using Raylib_cs;

public class Collectible
{

    int amountOfCollectibles = 20;

    //En array med typen texture2D
    Texture2D[] collectibleTextures;

    //Rektangel för collectibles
    public Rectangle collectibleRec;

    //Texturen för varje collectible
    public Texture2D texture;

    //En lista med alla collectibles 
    public Dictionary<int, Collectible> collectibles;

    //En lista med alla collectibles som ska förstöras
    public List<Collectible> collectiblesToDestroy;

    //Varje collectibles position anges som vector2 istället för att använda rektangelns x och y värden
    public Vector2 position
    {
        get => new Vector2(collectibleRec.x, collectibleRec.y);
        set
        {
            collectibleRec.x = value.X;
            collectibleRec.y = value.Y;
        }
    }

    public Collectible()
    {
        collectibles = new();

        collectibleTextures = new Texture2D[2];
        collectibleTextures[0] = Raylib.LoadTexture("Bilder/Battery.png");
        collectibleTextures[1] = Raylib.LoadTexture("Bilder/Fueltank.png");
        texture = collectibleTextures[Random.Shared.Next(0, collectibleTextures.Length)]; //Slumpmässig textur för varje collectible
        collectibleRec = new Rectangle(0, 0, texture.width, texture.height);
        position = new Vector2(0, 0);
    }

    //Metod som genererar alla collectibles
    public void GenerateCollectibles(CaveGeneration cave)
    {
        for (int x = 0; x < cave.tileGrid.GetLength(0); x++)
        {
            for (int y = 0; y < cave.tileGrid.GetLength(1); y++)
            {
                if (cave.tileGrid[x, y] == 0 && Random.Shared.Next(0, cave.worldSize * cave.worldSize) < 40 && collectibles.Count < amountOfCollectibles)
                    SpawnCollectible(new Collectible(), new Vector2((int)x * cave.worldSize, (int)y * cave.worldSize));
            }
        }
        //Kollar om det borde finnas en collectible på varje position i tileGrid
    }

    //Metod som spawnar collectibles på respektive position
    int coll = 1;
    private void SpawnCollectible(Collectible collectible, Vector2 position)
    {
        collectible.position = position;
        collectibles.Add(coll, collectible);
        coll++;
    }

    //Metod som ritar ut alla collectibles
    public void DrawCollectibles()
    {
        foreach (var kvp in collectibles)
        {

            Raylib.DrawTexture(kvp.Value.texture, (int)kvp.Value.position.X, (int)kvp.Value.position.Y, Color.WHITE);
        }
    }
}