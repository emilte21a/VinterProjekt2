using System.Dynamic;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using Raylib_cs;


public class Fighter
{
    public int hp;

    public Rectangle playerRect;

    public Vector2 playerPosition;

    Texture2D playerSprite = Raylib.LoadTexture("Bilder/CharacterSpriteSheet.png");

    public Camera2D camera { get; init; }

    public Fighter()
    {
        hp = 100;
        playerRect = new Rectangle(-50, 0, playerSprite.width / 6, playerSprite.height);
    }

    public float PlayerXmovement(float playerPos, float speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            playerPos -= speed * Raylib.GetFrameTime() * 50;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            playerPos += speed * Raylib.GetFrameTime() * 50;
        }

        return playerPos;
    }

    public float PlayerYmovement(float playerPos, float speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        {
            playerPos -= speed * Raylib.GetFrameTime() * 50;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        {
            playerPos += speed * Raylib.GetFrameTime() * 50;
        }

        return playerPos;
    }

    public enum PlayerDirection
    {
        Idle,
        Left,
        Right,
        Up,
        Down,
        RightUp,
        RightDown,
        LeftUp,
        LeftDown
    }

    public PlayerDirection GetPlayerDirection()
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            return PlayerDirection.Left;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            return PlayerDirection.Right;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            return PlayerDirection.Up;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            return PlayerDirection.Down;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D) && Raylib.IsKeyDown(KeyboardKey.KEY_W))
            return PlayerDirection.RightUp;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D) && Raylib.IsKeyDown(KeyboardKey.KEY_S))
            return PlayerDirection.RightDown;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A) && Raylib.IsKeyDown(KeyboardKey.KEY_W))
            return PlayerDirection.LeftUp;

        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A) && Raylib.IsKeyDown(KeyboardKey.KEY_S))
            return PlayerDirection.LeftDown;

        else
            return PlayerDirection.Idle;
    }

    List<Bullet> bullets = new();
    List<Bullet> bulletsToDestroy = new();

    public void Shoot(float _speed)
    {
        Vector2 mousePosition = Raylib.GetMousePosition() - camera.offset;
        Vector2 pos = new Vector2(playerRect.x, playerRect.y);
        Vector2 diff = mousePosition - pos;
        Vector2 bulletDirection = Vector2.Normalize(diff + pos);

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            bullets.Add(new Bullet(new Vector2(playerRect.x + playerRect.width / 2, playerRect.y + playerRect.height / 2), bulletDirection, _speed));

        foreach (var bullet in bullets)
        {
            bullet.Update();
            if (bullet.ShouldDestroy(playerRect))
            {
                bullets.RemoveAll(bullet => bulletsToDestroy.Contains(bullet));
                bulletsToDestroy.Clear();
            }
        }
    }

    public void DrawBullets()
    {
        foreach (var bullet in bullets)
            bullet.Draw();
    }

    int frame { get; set; }
    int timer = 0;
    public int DrawPlayer(int _maxFrames, int _timePerFrame)
    {
        timer++;
        if (timer > _timePerFrame)
        {
            frame++;
            timer = 0;
        }
        return frame %= _maxFrames;
    }

    public bool CheckCollision(CaveGeneration cave)
    {
        return cave.worldTiles.Any(worldTile => Raylib.CheckCollisionRecs(playerRect, worldTile.tileRect));
    }

    float intersectionWidth;
    float intersectionHeight;

    public void CalculateCollisionSize(PlayerDirection _direction, Rectangle _playerRectangle, Tile _collidingTile)
    {
        if (_direction == PlayerDirection.Right)
        {
            intersectionHeight = 0;
            intersectionWidth = Math.Min(_playerRectangle.x + _playerRectangle.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(_playerRectangle.x, _collidingTile.tileRect.x);
            playerRect.x -= intersectionWidth;
        }

        else if (_direction == PlayerDirection.Left)
        {
            intersectionHeight = 0;
            intersectionWidth = Math.Min(_playerRectangle.x + _playerRectangle.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(_playerRectangle.x, _collidingTile.tileRect.x);
            playerRect.x += intersectionWidth;
        }

        else if (_direction == PlayerDirection.Down)
        {
            intersectionWidth = 0;
            intersectionHeight = Math.Min(_playerRectangle.y + _playerRectangle.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(_playerRectangle.y, _collidingTile.tileRect.y);
            playerRect.y -= intersectionHeight;
        }

        else if (_direction == PlayerDirection.Up)
        {
            intersectionWidth = 0;
            intersectionHeight = Math.Min(_playerRectangle.y + _playerRectangle.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(_playerRectangle.y, _collidingTile.tileRect.y);
            playerRect.y += intersectionHeight;
        }

        // else if (_direction == PlayerDirection.RightUp)
        // {
        //     intersectionWidth = Math.Min(_playerRectangle.x + _playerRectangle.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(_playerRectangle.x, _collidingTile.tileRect.x);
        //     intersectionHeight = Math.Min(_playerRectangle.y + _playerRectangle.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(_playerRectangle.y, _collidingTile.tileRect.y);
        //     playerRect.x-=intersectionWidth;
        //     playerRect.y+=intersectionHeight;
        // }
        // else if (_direction == PlayerDirection.RightDown)
        // {
        //     intersectionWidth = Math.Min(_playerRectangle.x + _playerRectangle.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(_playerRectangle.x, _collidingTile.tileRect.x);
        //     intersectionHeight = Math.Min(_playerRectangle.y + _playerRectangle.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(_playerRectangle.y, _collidingTile.tileRect.y);
        //     playerRect.x-=intersectionWidth;
        //     playerRect.y-=intersectionHeight;
        // }
        // else if (_direction == PlayerDirection.LeftUp)
        // {
        //     intersectionWidth = Math.Min(_playerRectangle.x + _playerRectangle.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(_playerRectangle.x, _collidingTile.tileRect.x);
        //     intersectionHeight = Math.Min(_playerRectangle.y + _playerRectangle.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(_playerRectangle.y, _collidingTile.tileRect.y);
        //     playerRect.x+=intersectionWidth;
        //     playerRect.y+=intersectionHeight;
        // }
        // else if (_direction == PlayerDirection.LeftDown)
        // {
        //     intersectionWidth = Math.Min(_playerRectangle.x + _playerRectangle.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(_playerRectangle.x, _collidingTile.tileRect.x);
        //     intersectionHeight = Math.Min(_playerRectangle.y + _playerRectangle.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(_playerRectangle.y, _collidingTile.tileRect.y);
        //     playerRect.x+=intersectionWidth;
        //     playerRect.y-=intersectionHeight;
        // }
    }

    public void ResetPosition(CaveGeneration cave)
    {

        if (CheckCollision(cave))
        {
            var collidingTile = cave.worldTiles.FirstOrDefault(worldTile => Raylib.CheckCollisionRecs(playerRect, worldTile.tileRect));

            if (collidingTile != null)
            {
                PlayerDirection direction = GetPlayerDirection();
                CalculateCollisionSize(direction, playerRect, collidingTile);
            }
        }
        else
        {
            intersectionHeight = 0;
            intersectionWidth = 0;
        }
        System.Console.WriteLine("width:" + intersectionWidth + " height:" + intersectionHeight);
    }
}
