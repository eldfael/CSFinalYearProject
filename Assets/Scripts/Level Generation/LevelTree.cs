using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTree : MonoBehaviour
{
    LevelNode startNode;
    LevelNode currentNode;
    List<LevelNode> nodeList;
    private void Start()
    {
        startNode = new LevelNode();
        currentNode = startNode;

        nodeList = new List<LevelNode>();
        nodeList.Add(startNode);

        // Creating the longest route to the boss room
        int maxDepth = Random.Range(5, 8);
        for (int x = 0; x < maxDepth; x++)
        {   
            if (x < maxDepth - 1)
            {
                currentNode = currentNode.CreateChildNode(NodeType.NORMAL);
            }
            else
            {
                currentNode = currentNode.CreateChildNode(NodeType.BOSS);
            }
            nodeList.Add(currentNode);
        }
        

    }
}
