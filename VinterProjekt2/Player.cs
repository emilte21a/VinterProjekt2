using System.Dynamic;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using Microsoft.VisualBasic;
using Raylib_cs;


public class Player
{
    public int hp;

    public Rectangle playerRect;

    public Vector2 playerPosition;

    Texture2D playerSprite = Raylib.LoadTexture("Bilder/CharacterSpriteSheet.png");

    public Camera2D camera { get; init; }

    public Player()
    {
        hp = 100;
        playerRect = new Rectangle(-50, 0, playerSprite.width / 6, playerSprite.height);
    }

    public float MovePlayerX(float _playerPosX, float _speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            _playerPosX -= _speed * Raylib.GetFrameTime() * 50;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            _playerPosX += _speed * Raylib.GetFrameTime() * 50;
        }

        return _playerPosX;
    }

    public float MovePlayerY(float _playerPosY, float _speed)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        {
            _playerPosY -= _speed * Raylib.GetFrameTime() * 50;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        {
            _playerPosY += _speed * Raylib.GetFrameTime() * 50;
        }

        return _playerPosY;
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

        bool isKeyDownA = Raylib.IsKeyDown(KeyboardKey.KEY_A);
        bool isKeyDownD = Raylib.IsKeyDown(KeyboardKey.KEY_D);
        bool isKeyDownW = Raylib.IsKeyDown(KeyboardKey.KEY_W);
        bool isKeyDownS = Raylib.IsKeyDown(KeyboardKey.KEY_S);

        if (isKeyDownA && !isKeyDownW && !isKeyDownS && !isKeyDownD)
            return PlayerDirection.Left;

        else if (isKeyDownD && !isKeyDownW && !isKeyDownS && !isKeyDownA)
            return PlayerDirection.Right;

        else if (isKeyDownW && !isKeyDownA && !isKeyDownD && !isKeyDownS)
            return PlayerDirection.Up;

        else if (isKeyDownS && !isKeyDownA && !isKeyDownD && !isKeyDownW)
            return PlayerDirection.Down;

        else if (isKeyDownW && !isKeyDownA && isKeyDownD && !isKeyDownS)
            return PlayerDirection.RightUp;

        else if (isKeyDownS && !isKeyDownA && isKeyDownD && !isKeyDownW)
            return PlayerDirection.RightDown;

        else if (isKeyDownW && isKeyDownA && !isKeyDownD && !isKeyDownS)
            return PlayerDirection.LeftUp;

        else if (isKeyDownS && isKeyDownA && !isKeyDownD && !isKeyDownW)
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
    float elapsed = 0;
    public int DrawPlayer(int _maxFrames, int _timePerFrame)
    {
        elapsed += Raylib.GetFrameTime();
        if (elapsed > _timePerFrame)
        {
            frame++;
            elapsed = 0;
        }
        return frame %= _maxFrames;
    }

    //[Kollisioner]==========================================================================
    float intersectionWidth;
    float intersectionHeight;

    public void CalculateCollisionSize(PlayerDirection _direction, Tile _collidingTile)
    {
        switch (_direction)
        {
            case PlayerDirection.Right:
                // intersectionHeight = 0;
                // intersectionWidth = -1 * (Math.Min(playerRect.x + playerRect.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(playerRect.x, _collidingTile.tileRect.x));

                //intersectionWidth = Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).width;

                if (playerRect.x + playerRect.width > _collidingTile.tileRect.x)
                {
                    // intersectionWidth = -1 * (int)Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).width;
                    // intersectionHeight = 0;
                    playerRect.x = _collidingTile.tileRect.x - playerRect.width;
                }

                break;

            case PlayerDirection.Left:
                //intersectionHeight = 0;
                //intersectionWidth = Math.Min(playerRect.x + playerRect.width, _collidingTile.tileRect.x + _collidingTile.tileRect.width) - Math.Max(playerRect.x, _collidingTile.tileRect.x);
                //playerRect.x += Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).width;
                if (playerRect.x < _collidingTile.tileRect.x + _collidingTile.tileRect.width)
                {
                    // intersectionWidth = (int)Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).width;
                    // intersectionHeight = 0;
                    playerRect.x = _collidingTile.tileRect.x + _collidingTile.tileRect.width;
                }
                break;

            case PlayerDirection.Down:
                //intersectionWidth = 0;
                //intersectionHeight = -1 * (Math.Min(playerRect.y + playerRect.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(playerRect.y, _collidingTile.tileRect.y));
                // playerRect.y -= Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).height;
                if (playerRect.y + playerRect.height > _collidingTile.tileRect.y)
                {
                    // intersectionHeight = -1 * (int)Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).height;
                    // intersectionWidth = 0;
                    playerRect.y = _collidingTile.tileRect.y - playerRect.height;
                }
                break;

            case PlayerDirection.Up:
                //intersectionWidth = 0;
                //intersectionHeight = Math.Min(playerRect.y + playerRect.height, _collidingTile.tileRect.y + _collidingTile.tileRect.height) - Math.Max(playerRect.y, _collidingTile.tileRect.y);
                // playerRect.y += Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).height;
                if (playerRect.y < _collidingTile.tileRect.y + _collidingTile.tileRect.height)
                {
                    // intersectionHeight = (int)Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).height;
                    // intersectionWidth = 0;
                    playerRect.y = _collidingTile.tileRect.y + _collidingTile.tileRect.height;
                }
                break;

            case PlayerDirection.RightUp:

                break;

            case PlayerDirection.RightDown:
               
                break;
            case PlayerDirection.LeftUp:
                
                break;
            case PlayerDirection.LeftDown:
                
                break;


        }
        //System.Console.WriteLine(Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect).width);
        // playerRect.x += intersectionWidth;
        // playerRect.y += intersectionHeight;


    }

    private List<Tile> collidingTiles; //En lista med de Tiles som spelaren kolliderar med

    public List<Tile> CheckCollision(CaveGeneration cave)
    {
        return cave.worldTiles.Where(worldTile => Raylib.CheckCollisionRecs(playerRect, worldTile.tileRect)).ToList(); //Returnerar en lista med de Tiles som spelaren kolliderar med
    }

    public void ControlPlayerPosition(CaveGeneration cave, float _speed)
    {
        playerRect.x = MovePlayerX(playerRect.x, _speed);
        playerRect.y = MovePlayerY(playerRect.y, _speed);

        collidingTiles = CheckCollision(cave);

        foreach (var colTile in collidingTiles)
        {
            //var collidingTile = cave.worldTiles.FirstOrDefault(worldTile => Raylib.CheckCollisionRecs(_playerRect, worldTile.tileRect));

            if (colTile != null)
            {
                PlayerDirection direction = GetPlayerDirection();
                CalculateCollisionSize(direction, colTile);
            }
        }


        //System.Console.WriteLine("width:" + intersectionWidth + " height:" + intersectionHeight);
    }
}
