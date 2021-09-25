using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Coord
{
    public int X;
    public int Y;

    public Coord()
    {

    }

    public Coord Up => new Coord(X, Y + 1);
    public Coord Down => new Coord(X, Y - 1);
    public Coord Right => new Coord(X + 1, Y);
    public Coord Left => new Coord(X - 1, Y);

    public List<Coord> Neighbor()
    {
        List<Coord> coords = new List<Coord>();
        coords.Add(Up);
        coords.Add(Down);
        coords.Add(Right);
        coords.Add(Left);
        return coords;
    }

    public Coord RandomNeighbor()
    {
        return FormulaHelper.RandomValueInList(Neighbor());
    }

    public Coord(int _x, int _y)
    {
        X = _x;
        Y = _y;
    }

    public Coord Clone()
    {
        Coord clone = new Coord();
        clone.X = X;
        clone.Y = Y;

        return clone;
    }
}
