using System.Numerics;
using Raylib_cs;

public class Collectible
{

    int amountOfCollectibles = 20;

    Texture2D[] collectibleTextures;
    public Rectangle collectibleRec;

    public Texture2D texture;

    public List<Collectible> collectibles;
    public List<Collectible> collectiblesToDestroy;

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
        texture = collectibleTextures[Random.Shared.Next(0, collectibleTextures.Length)];
        collectibleRec = new Rectangle(0, 0, texture.width, texture.height);
        position = new Vector2(0, 0);
    }


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
    }

    private void SpawnCollectible(Collectible _collectible, Vector2 _position)
    {
        _collectible.position = _position;
        collectibles.Add(_collectible);
    }

    public void DrawCollectibles()
    {
        for (int i = 0; i < collectibles.Count; i++)
            Raylib.DrawTexture(collectibles[i].texture, (int)collectibles[i].position.X, (int)collectibles[i].position.Y, Color.WHITE);
    }   
}