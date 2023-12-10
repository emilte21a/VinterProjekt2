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

    public Texture2D playerSprite = Raylib.LoadTexture("Bilder/CharacterSpriteSheet.png");

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

        else if ((!isKeyDownS && !isKeyDownA && !isKeyDownD && isKeyDownW) || (!isKeyDownS && isKeyDownA && isKeyDownD && isKeyDownW))
            return PlayerDirection.Up;

        else if ((isKeyDownS && !isKeyDownA && !isKeyDownD && !isKeyDownW) || (isKeyDownS && isKeyDownA && isKeyDownD && !isKeyDownW))
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

    public void CalculateCollisionSize(PlayerDirection _direction, Tile _collidingTile)
    {
        var collisionRec = Raylib.GetCollisionRec(playerRect, _collidingTile.tileRect);

        switch (_direction)
        {
            case PlayerDirection.Right:
                playerRect.x -= (int)collisionRec.width;

                break;

            case PlayerDirection.Left:
                playerRect.x += (int)collisionRec.width;
                break;

            case PlayerDirection.Down:
                playerRect.y -= (int)collisionRec.height;
                break;

            case PlayerDirection.Up:
                playerRect.y += (int)collisionRec.height;
                break;

            case PlayerDirection.RightUp:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y += (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x -= (int)collisionRec.width;

                break;

            case PlayerDirection.RightDown:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y -= (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x -= (int)collisionRec.width;

                break;

            case PlayerDirection.LeftUp:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y += (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x += (int)collisionRec.width;

                break;

            case PlayerDirection.LeftDown:
                if ((int)collisionRec.height < (int)collisionRec.width)
                    playerRect.y -= (int)collisionRec.height;

                else if ((int)collisionRec.height > (int)collisionRec.width)
                    playerRect.x += (int)collisionRec.width;

                break;

            case PlayerDirection.Idle:
                

                break;
        }
    }

    public List<Tile> collidingTiles; //En lista med de Tiles som spelaren kolliderar med

    public List<Tile> CheckCollision(CaveGeneration cave)
    {
        return cave.worldTiles.Where(worldTile => Raylib.CheckCollisionRecs(playerRect, worldTile.tileRect)).ToList(); //Returnerar en lista med de Tiles som spelaren kolliderar med
    }

    public void ControlPlayerPosition(CaveGeneration cave, float _speed)
    {

        playerRect.x = (int)MovePlayerX(playerRect.x, _speed);
        playerRect.y = (int)MovePlayerY(playerRect.y, _speed);

        collidingTiles = CheckCollision(cave);

        PlayerDirection direction = GetPlayerDirection();

        foreach (var colTile in collidingTiles)
        {
            if (colTile != null)
            {
                CalculateCollisionSize(direction, colTile);
            }
        }
    }
}
