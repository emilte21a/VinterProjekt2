using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using Raylib_cs;

public class CameraSmooth
{

    public Vector2 Lerp(Vector2 _start, Vector2 _end, float _time)
    {
        return new Vector2(_start.X + _time * (_end.X - _start.X), _start.Y + _time * (_end.Y - _start.Y));
    }
}