using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using Raylib_cs;

public class UniversalMath
{
    //Metod som returnerar en vector2 som använder Linear Interpolation 
    public Vector2 Lerp(Vector2 _start, Vector2 _end, float _time)
    {
        return new Vector2(_start.X + _time * (_end.X - _start.X), _start.Y + _time * (_end.Y - _start.Y));
    }

    //Metod som räknar ut avståndet mellan två vector2 (Jag vet inte varför jag gjorde en metod av detta)
    public float Distance(Vector2 _from, Vector2 _to)
    {
        return Vector2.Distance(_from, _to);
    }
}