using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTree : MonoBehaviour
{
    LevelNode startNode;
    LevelNode currentNode;
    List<LevelNode> nodeList;
    List<GameObject> roomList;

    LevelNode[,] nodeGrid;

    private void Start()
    {
        roomList = new List<GameObject>();

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
                currentNode = currentNode.CreateChildNode(RoomType.NORMAL);
            }
            else
            {
                currentNode = currentNode.CreateChildNode(RoomType.BOSS);
            }
            nodeList.Add(currentNode);
        }

        nodeGrid = new LevelNode[(maxDepth*2)+1,(maxDepth*2)+1];

        Vector2 pos = new Vector2(maxDepth, maxDepth);

        foreach (LevelNode node in nodeList)
        {
            GameObject room = new GameObject();
            
            room.name = "Room";
            room.AddComponent<Room>();
            
            node.SetRoom(room);

            roomList.Add(room);
            
            if (node.GetParentNode() != null)
            {
                pos = node.GetParentNode().GetPosition();

                bool placed = false;
                for (int x = 0; x < 20; x++) // Number of placement attempts before giving up
                {

                }

            }

            node.SetPosition(pos);
            nodeGrid[(int)pos.x,(int)pos.y] = node;

        }

        

    }
}
