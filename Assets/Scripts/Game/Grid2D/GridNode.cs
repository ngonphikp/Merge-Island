using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : GridNodeBase
{
    [SerializeField] Collider2D m_Collider;

    protected override void Start()
    {
        base.Start();
        GameCore.Event.Subscribe<SelectedTowerEventArgs>(OnSelectedTowerEventHandler);
        GameCore.Event.Subscribe<DeselectedTowerEventArgs>(OnDeselectedTowerEventHandler);

        GameCore.Event.Subscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Subscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameCore.Event.Unsubscribe<SelectedTowerEventArgs>(OnSelectedTowerEventHandler);
        GameCore.Event.Unsubscribe<DeselectedTowerEventArgs>(OnDeselectedTowerEventHandler);

        GameCore.Event.Unsubscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Unsubscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    private void OnSelectedTowerEventHandler(object sender, IEventArgs e)
    {
        m_Collider.enabled = true;
    }

    private void OnDeselectedTowerEventHandler(object sender, IEventArgs e)
    {
        m_Collider.enabled = false;
    }

    private void OnUnAnchorEventHandler(object sender, IEventArgs e)
    {
        m_Collider.enabled = true;
    }

    private void OnAnchorEventHandler(object sender, IEventArgs e)
    {
        m_Collider.enabled = false;
    }

    public override void Init(Node _node)
    {
        base.Init(_node);
        name = $"Node[{_node.Coord.X};{_node.Coord.Y}]";
        Trans.position = NodeCoordinate() + MainMode.S.PlayerPosition;

        m_Collider.enabled = MainMode.S.Status == ModeStatus.UnAnchor;
    }    
}
