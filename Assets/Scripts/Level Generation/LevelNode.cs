using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NodeType 
{ 
    NORMAL,ITEM,BOSS,START
}
public class LevelNode
{
    LevelNode parentNode;
    List<LevelNode> childrenNodes;

    NodeType nodeType;
    int depth;

    public LevelNode()
    {
        parentNode = null; // The starting room
        nodeType = NodeType.START;

        childrenNodes = new List<LevelNode>();
        depth = 0;
    }

    public LevelNode(LevelNode _parentNode, NodeType _nodeType)
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
    public LevelNode CreateChildNode(NodeType _nodeType)
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
    public NodeType GetNodeType()
    {
        return nodeType;
    }
    public void SetNodeType(NodeType _nodeType)
    {
        nodeType = _nodeType;
    }
    public int GetDepth()
    {
        return depth;
    }
}
