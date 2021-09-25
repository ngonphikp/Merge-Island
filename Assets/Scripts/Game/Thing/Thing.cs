using Core;
using Lean.Touch;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Thing : MonoBehaviour
{
    private GridNodeBase m_GridNode;
    private GridNodeBase m_OldGridNode;
    private Transform m_Trans;
    private ThingType m_Type;
    private int m_level;

    [SerializeField] Collider2D m_Collider;
    [SerializeField] LeanSelectableByFinger m_Select;

    public GridNodeBase GridNode => m_GridNode;
    public ThingType Type => m_Type;
    public int Level => m_level;

    private SortingGroup m_layer;

    private void Awake()
    {
        m_Trans = transform;
        m_layer = GetComponent<SortingGroup>();
    }

    private void OnEnable()
    {
        GameCore.Event.Subscribe<SelectedTowerEventArgs>(OnSelectedTowerEventHandler);
        GameCore.Event.Subscribe<DeselectedTowerEventArgs>(OnDeselectedTowerEventHandler);

        GameCore.Event.Subscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Subscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    private void OnDisable()
    {
        GameCore.Event.Unsubscribe<SelectedTowerEventArgs>(OnSelectedTowerEventHandler);
        GameCore.Event.Unsubscribe<DeselectedTowerEventArgs>(OnDeselectedTowerEventHandler);

        GameCore.Event.Unsubscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Unsubscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    public void Init(int _level)
    {
        m_Select.enabled = false;
        m_layer.sortingOrder = -9;

        m_level = _level;
    }

    public void SetNode(GridNodeBase _gridNode)
    {
        m_GridNode = _gridNode;
        m_GridNode.SetThing(this);
        m_Trans.position = _gridNode.NodeCoordinate() + MainMode.S.PlayerPosition;

        m_OldGridNode = m_GridNode;

        m_Select.enabled = MainMode.S.Status == ModeStatus.Anchor;
        m_layer.sortingOrder = 3;
    }

    public void SetType(ThingType _type)
    {
        m_Type = _type;
    }

    private void OnSelectedTowerEventHandler(object sender, IEventArgs e)
    {
        m_Collider.enabled = false;
    }

    private void OnDeselectedTowerEventHandler(object sender, IEventArgs e)
    {
        m_Collider.enabled = true;
    }

    private void OnUnAnchorEventHandler(object sender, IEventArgs e)
    {
        m_Select.enabled = false;
    }

    private void OnAnchorEventHandler(object sender, IEventArgs e)
    {
        m_Select.enabled = true;
    }

    public void OnSelectedHandler()
    {
        m_OldGridNode = m_GridNode;

        m_GridNode.SetThing(null);
        m_GridNode = null;

        GameCore.Event.Fire(this, SelectedTowerEventArgs.Create());
    }

    public void OnDeselectedHandler()
    {
        GameCore.Event.Fire(this, DeselectedTowerEventArgs.Create());
    }

    public void OnDropSuccessHandler(GameObject other)
    {
        //Debug.Log("OnDropSuccessHandler: " + other.name);
       
        var otherNode = other.GetComponent<GridNodeBase>();
        if (otherNode && !otherNode.Equals(m_OldGridNode))
        {
            if (otherNode.Thing)
            {                
                if (otherNode.Thing.Type == m_Type && otherNode.Thing.Level == m_level && m_level < 3)
                {
                    var things = MainMode.S.GetNeighbor(otherNode.Thing);
                    string key = FormulaHelper.Coord2Key(m_OldGridNode.Node.Coord);
                    if (!things.ContainsKey(key) && things.Count >= 2)
                    {
                        things.Add(key, this);
                    }

                    if(things.Count >= 3)
                    {
                        Merge(otherNode);
                    }
                    else
                    {
                        Swap(otherNode);
                    }
                }
                else
                {
                    Swap(otherNode);
                }
            }
            else
            {
                Teleport(otherNode);
            }
        }
        else
        {
            Report();
        }
    }

    private void Merge(GridNodeBase otherNode)
    {
        //Debug.Log("Merge");
        m_GridNode = m_OldGridNode;

        m_GridNode.SetThing(null);
        MainMode.S.MergeThing(this, otherNode.Thing);
    }

    private void Swap(GridNodeBase otherNode)
    {
        //Debug.Log("Swap");
        m_GridNode = m_OldGridNode;
        m_GridNode.SetThing(null);

        var otherThing = otherNode.Thing;
        otherNode.SetThing(null);
        SetNode(otherNode);

        MainMode.S.TeleThing(otherThing, otherNode.Node.Coord);        
    }

    private void Teleport(GridNodeBase otherNode)
    {
        //Debug.Log("Teleport");
        m_GridNode = m_OldGridNode;
        m_GridNode.SetThing(null);

        SetNode(otherNode);
    }

    private void Report()
    {
        //Debug.Log("Report");
        SetNode(m_OldGridNode);
    }

    public void OnDropFailureHandler()
    {
        //Debug.Log("OnDropFailureHandler");
        SetNode(m_OldGridNode);
    }

    [Button]
    public void Kill()
    {
        m_GridNode.SetThing(null);
        Destroy(gameObject);
    }

    private void Update()
    {
        //if(m_GridNode != null && (m_GridNode.Thing == null || (m_GridNode.Thing != null && m_GridNode.Thing != this)))
        //{
        //    Debug.LogWarning("Warning: " + gameObject.name + " >< " + m_GridNode.gameObject.name);
        //}
    }
}
