using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using Raylib_cs;

public class UniversalMath
{
    public Vector2 Lerp(Vector2 _start, Vector2 _end, float _time)
    {
        return new Vector2(_start.X + _time * (_end.X - _start.X), _start.Y + _time * (_end.Y - _start.Y));
    }

    public float Distance(Vector2 _from, Vector2 _to)
    {

        return Vector2.Distance(_to, _from);
    }


}