using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodeBase : MonoBehaviour
{
    private Node m_Node;
    private Transform m_Trans;
    private Thing m_Thing;

    public Node Node => m_Node;
    public Transform Trans => m_Trans;
    public Thing Thing => m_Thing;

    public bool Available
    {
        get { return m_Thing == null; }
    }

    protected virtual void Awake()
    {
        m_Trans = transform;
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    public virtual void Init(Node _node)
    {
        m_Node = _node;
    }

    public void SetThing(Thing _thing)
    {
        m_Thing = _thing;
    }

    public Vector2 NodeCoordinate()
    {
        return new Vector2(m_Node.XCoord, m_Node.YCoord);
    }

    private void Update()
    {
        //if (m_Thing != null && (m_Thing.GridNode == null || (m_Thing.GridNode != null && m_Thing.GridNode != this)))
        //{
        //    Debug.LogWarning("Warning: " + gameObject.name + " >< " + m_Thing.gameObject.name);
        //}
    }
}
