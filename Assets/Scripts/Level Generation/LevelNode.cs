using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomType 
{ 
    NORMAL,ITEM,BOSS,START
}
public class LevelNode
{
    LevelNode parentNode;
    List<LevelNode> childrenNodes;

    RoomType nodeType;
    int depth;

    GameObject room;
    Vector2 position;

    public LevelNode()
    {
        parentNode = null; // The starting room
        nodeType = RoomType.START;

        childrenNodes = new List<LevelNode>();
        depth = 0;
    }

    public LevelNode(LevelNode _parentNode, RoomType _nodeType)
    {
        parentNode = _parentNode;
        nodeType = _nodeType;

        childrenNodes = new List<LevelNode>();
        depth = parentNode.GetDepth() + 1;
    }

    public LevelNode GetParentNode()
    {
        return parentNode;
    }
    public LevelNode CreateChildNode(RoomType _nodeType)
    {
        LevelNode childNode = new LevelNode(this, _nodeType);
        childrenNodes.Add(childNode);
        return childNode;
        
    }
    public LevelNode GetChildNode(int index)
    {
        return childrenNodes[index];
    }
    public int GetNumberOfChildren()
    {
        return childrenNodes.Capacity;
    }
    public RoomType GetNodeType()
    {
        return nodeType;
    }
    public void SetNodeType(RoomType _nodeType)
    {
        nodeType = _nodeType;
    }
    public int GetDepth()
    {
        return depth;
    }

    public GameObject GetRoom()
    {
        return room;
    }
    public void SetRoom(GameObject _room)
    {
        room = _room;
    }
    public void SetPosition(Vector2 _position)
    {
        position = _position;
    }
    public Vector2 GetPosition()
    {
        return position;
    }
}
