using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormulaHelper
{
    public static string Coord2Key(Coord _coord)
    {
        return (_coord.X + "_" + _coord.Y);
    }

    public static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }

        return result;
    }

    public static int IntParseFast(char value)
    {
        int result = 0;
        result = 10 * result + (value - 48);
        return result;
    }

    public static T RandomValueInList<T>(List<T> lst)
    {
        if (lst.Count > 0) return lst[Random.Range(0, lst.Count)];
        return default;
    }

    public static void Shuffle<T>(this List<T> idxs)
    {
        for (int i = 0; i < idxs.Count - 1; i++)
        {
            int random = Random.Range(i, idxs.Count);
            idxs.Swap(i, random);
        }
    }

    private static void Swap<T>(this List<T> idxs, int a, int b)
    {
        T temp = idxs[a];
        idxs[a] = idxs[b];
        idxs[b] = temp;
    }
}
