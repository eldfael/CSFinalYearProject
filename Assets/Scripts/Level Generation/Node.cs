using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    Node creator;
    NodeType type;
    Vector2 position;

    public Node(Node _creator, NodeType _type, Vector2 _position)
    {
        creator = _creator;
        type = _type;
        position = _position;
    }

    public Node(NodeType _type, Vector2 _position)
    {
        creator = null;
        type = _type;
        position = _position;
    }

    public Node(Vector2 _position)
    {
        creator = null;
        type = NodeType.EMPTY;
        position = _position;
    }

    public NodeType GetNodeType() { return type; }
    public Node GetCreator() { return creator; }
    public Vector2 GetPosition() { return position; }

    public void SetNodeType(NodeType _type) { type = _type; }

}

public enum NodeType
{ 
    EMPTY,OPEN,WALL,GROWN,ROOM
}
