using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using Raylib_cs;

public class CameraSmooth
{

    public Vector2 Lerp(Vector2 _vector1, Vector2 _vector2, float time)
    {
        return _vector1 + ((_vector2 - _vector1) * time);
    }
}