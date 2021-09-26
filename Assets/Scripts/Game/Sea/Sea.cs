using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sea : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] Vector2 minCoord;
    [SerializeField] Vector2 maxCoord;
    [SerializeField] Vector2 offset = new Vector2(1, 1.2f);

    [SerializeField] int maxCount = 100;
    [SerializeField] float timeSpawn = 1f;
    float time = 0f;

    List<ThingType> types = new List<ThingType>() { ThingType.Wood, ThingType.Chest, ThingType.Treasure };

    private void Start()
    {
        inpTimeSpawn.text = timeSpawn.ToString();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= timeSpawn)
        {
            time = 0;
            GenThing();
        }        
    }

    [SerializeField] TMP_InputField inpTimeSpawn;

    public void SetTimeSpawn()
    {
        timeSpawn = Mathf.Max(0.1f, float.Parse(inpTimeSpawn.text));
    }

    public void GenThing()
    {
        if (content.childCount > maxCount) return;

        Vector2 minCoordDanger = new Vector2(MainMode.S.PlayerMinCoord.x - offset.x, MainMode.S.PlayerMinCoord.y - offset.y);
        Vector2 maxCoordDanger = new Vector2(MainMode.S.PlayerMaxCoord.y + offset.x, MainMode.S.PlayerMaxCoord.y + offset.y);

        Vector2 position = new Vector2();
        while (true)
        {
            position.x = Random.Range(minCoord.x, maxCoord.x);
            position.y = Random.Range(minCoord.y, maxCoord.y);

            if (position.x > maxCoordDanger.x) break;
            if (position.x < minCoordDanger.x) break;
            if (position.y > maxCoordDanger.y) break;
            if (position.y < minCoordDanger.y) break;
        }        

        var thing = CreateThing(FormulaHelper.RandomValueInList(types), 0);

        thing.gameObject.transform.position = position;
    }

    [Button]
    public Thing CreateThing(ThingType _type, int _level)
    {
        string path = "Thing/" + _type + "_" + _level;
        GameObject prefab = Resources.Load<GameObject>(path);

        var thing = CreateBaseThing(prefab, _level);
        thing.SetType(_type);

        return thing;
    }

    private Thing CreateBaseThing(GameObject prefab, int _level)
    {
        var clone = Instantiate(prefab, content);
        var thing = clone.GetComponent<Thing>();
        thing.Init(_level);
        return thing;
    }

    [Button]
    public void Clear()
    {
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
    }
}
