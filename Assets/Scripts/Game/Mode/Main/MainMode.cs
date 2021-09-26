using Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMode : GameCore
{
    private static MainMode m_Self;
    private ModeStatus m_Status;

    public static MainMode S => m_Self;
    public ModeStatus Status => m_Status;

    [SerializeField] CameraControl m_Camera;
    [SerializeField] Player m_Player;
    public Vector2 PlayerPosition => m_Player.transform.position;
    public Vector2 PlayerMinCoord => new Vector2(m_Player.transform.position.x + m_Island.MinCoord.X, m_Player.transform.position.y + m_Island.MinCoord.Y);
    public Vector2 PlayerMaxCoord => new Vector2(m_Player.transform.position.x + m_Island.MaxCoord.X, m_Player.transform.position.y + m_Island.MaxCoord.Y);

    [SerializeField] Island m_Island;
    [SerializeField] List<Coord> m_DataLand;

    [SerializeField] Things m_Thing;
    [SerializeField] List<ThingSave> m_ThingsSave;

    Dictionary<string, Thing> m_Neighbor = new Dictionary<string, Thing>();

    private void Awake()
    {
        m_Self = this;
    }

    protected override void OnModeStart()
    {
        m_Status = ModeStatus.Anchor;
        m_Camera.Init();
        m_Player.Init();
        m_Island.Init();

        GenIsLand();
        GenThings();
    }

    protected override void OnModeUpdate()
    {

    }

    protected override void OnModeFixedUpdate()
    {

    }

    protected override void OnModeDestroy()
    {

    }

    public void GenThing()
    {
        GenThing(new Coord(), (ThingType)Random.Range(0, 5), 0);
    }

    public void GenLand()
    {
        var coord = m_Island.GetAvailableCoord();
        if(coord != null) GenLand(coord);
    }

    public void Anchor()
    {
        m_Status = ModeStatus.Anchor;

        Event.Fire(this, AnchorEventArgs.Create());
    }

    public void UnAnchor()
    {
        m_Status = ModeStatus.UnAnchor;

        Event.Fire(this, UnAnchorEventArgs.Create());
    }

    public void TeleThing(Thing thing, Coord _coord)
    {
        var node = m_Island.GetAvailableNode(_coord);
        if (node)
        {
            thing.SetNode(node);
        }
        else
        {
            Debug.Log("Null");
        }
    }

    public void MergeThing(Thing selfThing, Thing otherThing)
    {
        int count = m_Neighbor.Count;
        int natural5 = count / 5;
        int remain5 = count - natural5 * 5;
        int natural3 = remain5 / 3;

        int up = natural5 * 2 + natural3;
        int old = remain5 - natural3 * 3;

        Debug.Log("Merge: " + count + " => " + up + " Up, " + old + " Old");
        if (up >= 1)
        {
            m_Neighbor.Remove(FormulaHelper.Coord2Key(selfThing.GridNode.Node.Coord));
            m_Neighbor.Remove(FormulaHelper.Coord2Key(otherThing.GridNode.Node.Coord));

            foreach (KeyValuePair<string, Thing> item in m_Neighbor)
            {
                item.Value.Kill();
            }
            m_Neighbor.Clear();

            selfThing.GridNode.SetThing(null);
            otherThing.GridNode.SetThing(null);

            for (int i = 0; i < up; i++)
            {
                GenThing(otherThing.GridNode.Node.Coord, selfThing.Type, selfThing.Level + 1);
            }
        }

        if (old >= 1)
        {
            TeleThing(selfThing, otherThing.GridNode.Node.Coord);
        }
        else
        {
            Destroy(selfThing.gameObject);
        }
        if (old >= 2)
        {
            TeleThing(otherThing, otherThing.GridNode.Node.Coord);
        }
        else
        {
            Destroy(otherThing.gameObject);
        }
    }

    public Dictionary<string, Thing> GetNeighbor(Thing thing)
    {
        m_Neighbor.Clear();

        FindNeighbor(thing);

        return m_Neighbor;
    }

    private void FindNeighbor(Thing thing)
    {
        var things = m_Island.GetThingNode(thing.GridNode.Node.Coord, thing.Type, thing.Level);
        for (int i = 0; i < things.Count; i++)
        {
            string key = FormulaHelper.Coord2Key(things[i].Node.Coord);
            if (!m_Neighbor.ContainsKey(key))
            {
                m_Neighbor.Add(key, things[i].Thing);
                FindNeighbor(things[i].Thing);                
            }
        }        
    }

    [Button]
    public void GenThing(Coord _coord, ThingType _type, int _level)
    {
        var node = m_Island.GetAvailableNode(_coord);
        if (node)
        {
            m_Thing.CreateThing(node, _type, _level);
        }
        else
        {
            Debug.Log("Full");
        }
    }

    [Button]
    private void GenLand(Coord _coord)
    {
        m_Island.Generate(_coord);
    }

    [Button]
    private void GenIsLand()
    {
        for (int i = 0; i < m_DataLand.Count; i++)
        {
            GenLand(m_DataLand[i]);
        }
    }

    [Button]
    private void GenThings()
    {
        for (int i = 0; i < m_ThingsSave.Count; i++)
        {
            GenThing(m_ThingsSave[i].Coord, m_ThingsSave[i].Type, m_ThingsSave[i].Level);
        }
    }
}
