using Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Island : MonoBehaviour
{
    [SerializeField] GameObject m_NodePrefab;
    [SerializeField] Vector2 m_NodeSize;
    [SerializeField] Vector2 m_Spacing;

    Dictionary<string, GridNodeBase> m_DicNode = new Dictionary<string, GridNodeBase>();
    Coord m_MaxCoord = new Coord(int.MinValue, int.MinValue);
    Coord m_MinCoord = new Coord(int.MaxValue, int.MaxValue);

    public Coord MaxCoord => m_MaxCoord;
    public Coord MinCoord => m_MinCoord;

    private Rigidbody2D rb;
    private CompositeCollider2D cc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CompositeCollider2D>();
    }

    public void Init()
    {
        Destroy(cc);
        Destroy(rb);

        GameCore.Event.Subscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Subscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    private void OnDestroy()
    {
        GameCore.Event.Unsubscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Unsubscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    private void OnUnAnchorEventHandler(object sender, IEventArgs e)
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        cc = gameObject.AddComponent<CompositeCollider2D>();
        cc.isTrigger = true;
    }

    private void OnAnchorEventHandler(object sender, IEventArgs e)
    {
        Destroy(cc);
        Destroy(rb);
    }

    public Coord GetAvailableCoord()
    {
        while (true)
        {
            string key = FormulaHelper.RandomValueInList(m_DicNode.Keys.ToList());
            var coord = m_DicNode[key].Node.Coord.RandomNeighbor();
            if (!m_DicNode.ContainsKey(FormulaHelper.Coord2Key(coord))) return coord;
        }
    }

    public GridNodeBase GetAvailableNode(Coord _coord)
    {
        if (CheckAvailableNode(_coord))
        {
            return m_DicNode[FormulaHelper.Coord2Key(_coord)];
        }
        else
        {
            int right = m_MaxCoord.X - _coord.X;
            int left = _coord.X - m_MinCoord.X;
            int up = m_MaxCoord.Y - _coord.Y;
            int down = _coord.Y - m_MinCoord.Y;

            int maxDis = Mathf.Max(right, left, up, down);
            for (int dis = 1; dis <= maxDis; dis++)
            {
                for (int x = _coord.X - dis; x <= _coord.X + dis; x++)
                {
                    if ((x == _coord.X - dis) || (x == _coord.X + dis))
                    {
                        for (int y = _coord.Y - dis; y <= _coord.Y + dis; y++)
                        {
                            var find = new Coord(x, y);

                            if (CheckAvailableNode(find))
                            {
                                return m_DicNode[FormulaHelper.Coord2Key(find)];
                            }
                        }
                    }
                    else
                    {
                        var min = new Coord(x, _coord.Y - dis);

                        if (CheckAvailableNode(min))
                        {
                            return m_DicNode[FormulaHelper.Coord2Key(min)];
                        }

                        var max = new Coord(x, _coord.Y + dis);

                        if (CheckAvailableNode(max))
                        {
                            return m_DicNode[FormulaHelper.Coord2Key(max)];
                        }
                    }
                }
            }
        }

        return null;
    }

    public List<GridNodeBase> GetThingNode(Coord _coord, ThingType _type, int _level)
    {
        List<GridNodeBase> result = new List<GridNodeBase>();

        List<Coord> coords = _coord.Neighbor();
        for (int i = 0; i < coords.Count; i++)
        {
            if (CheckThingNode(coords[i], _type, _level))
            {
                result.Add(m_DicNode[FormulaHelper.Coord2Key(coords[i])]);
            }
        }

        return result;
    }

    private bool CheckThingNode(Coord _coord, ThingType _type, int _level)
    {
        string key = FormulaHelper.Coord2Key(_coord);
        return m_DicNode.ContainsKey(key) && !m_DicNode[key].Available && m_DicNode[key].Thing && m_DicNode[key].Thing.Type == _type && m_DicNode[key].Thing.Level == _level;
    }

    private bool CheckAvailableNode(Coord _coord)
    {
        string key = FormulaHelper.Coord2Key(_coord);
        return m_DicNode.ContainsKey(key) && m_DicNode[key].Available;
    }

    public void Generate(Coord _coord)
    {
        Node model = new Node()
        {
            Coord = _coord.Clone(),
            XCoord = m_NodeSize.x * _coord.X + _coord.X * m_Spacing.x,
            YCoord = m_NodeSize.x * _coord.Y + _coord.Y * m_Spacing.y
        };
        var node = CreateNode(model);

        m_DicNode.Add(_coord.X + "_" + _coord.Y, node);

        m_MaxCoord.X = Mathf.Max(m_MaxCoord.X, _coord.X);
        m_MaxCoord.Y = Mathf.Max(m_MaxCoord.Y, _coord.Y);
        m_MinCoord.X = Mathf.Min(m_MinCoord.X, _coord.X);
        m_MinCoord.Y = Mathf.Min(m_MinCoord.Y, _coord.Y);
    }

    private GridNodeBase CreateNode(Node _node)
    {
        var clone = Instantiate(m_NodePrefab, transform);
        var node = clone.GetComponent<GridNodeBase>();
        node.Init(_node);
        return node;
    }

    [Button]
    public void Clear()
    {
        m_DicNode.Clear();
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.CompareTag("Thing"))
        {
            var node = GetAvailableNode(new Coord(0, 0));
            if (node)
            {
                var thing = collision.gameObject.GetComponent<Thing>();
                MainMode.S.GenThing(node.Node.Coord, thing.Type, thing.Level);
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Full");
            }
        }
    }
}
