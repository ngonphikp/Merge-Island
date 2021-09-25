using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThingSave
{
    public ThingType Type;
    public int Level;
    public Coord Coord = new Coord();

    public ThingSave()
    {

    }

    public ThingSave Clone()
    {
        var clone = new ThingSave();
        clone.Type = Type;
        clone.Coord = Coord.Clone();

        return clone;
    }
}
