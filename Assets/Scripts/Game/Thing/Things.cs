using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Things : MonoBehaviour
{
    public Thing CreateThing(GridNodeBase _node, ThingType _type, int _level)
    {
        string path = "Thing/" + _type + "_" + _level;
        GameObject prefab = Resources.Load<GameObject>(path);

        var thing = CreateBaseThing(prefab, _node, _level);
        thing.SetType(_type);

        return thing;
    }

    private Thing CreateBaseThing(GameObject prefab, GridNodeBase _node, int _level)
    {
        var clone = Instantiate(prefab, transform);
        var thing = clone.GetComponent<Thing>();
        thing.Init(_level);
        thing.SetNode(_node);        
        return thing;
    }

    [Button]
    public void Clear()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }
}
