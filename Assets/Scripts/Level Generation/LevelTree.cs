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
    public GameObject roomObject;

    int ROOMSIZE = 26;

    private void Start()
    {
        roomList = new List<GameObject>();

        startNode = new LevelNode();
        currentNode = startNode;

        nodeList = new List<LevelNode>();
        nodeList.Add(startNode);

        // Creating the longest route to the boss room
        int maxDepth = Random.Range(6, 9);
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

        
        // Create branches
        for (int b = 0; b < Random.Range(1, 4); b++)
        {
            bool loop = true;
            int branchDepth = 0;

            while (loop)
            {
                branchDepth = Random.Range(1, 4);
                currentNode = nodeList[Random.Range(1, maxDepth - 1)];

                if (currentNode.GetDepth() + branchDepth <= maxDepth && currentNode.GetNumberOfChildren() > 0 && currentNode.GetNumberOfChildren() < 3)
                {
                    loop = false;
                }
            }

            for (int x = 0; x < branchDepth; x++)
            {
                currentNode = currentNode.CreateChildNode(RoomType.NORMAL);
                nodeList.Add(currentNode);
            }
        }
        

        nodeGrid = new LevelNode[(maxDepth*2)+1,(maxDepth*2)+1];

        Vector2 pos = new Vector2(maxDepth, maxDepth);

        foreach (LevelNode node in nodeList)
        {
            GameObject room = Instantiate(roomObject);

            room.GetComponent<Room>().SetRoomType(node.GetNodeType());

            node.SetRoom(room);

            roomList.Add(room);

            bool placed = false;

            if (node.GetParentNode() != null)
            {
                pos = node.GetParentNode().GetPosition();
                Debug.Log(pos);

                int maxAttempts = 20;
                for (int x = 0; x < maxAttempts; x++) // Number of placement attempts before giving up
                {
                    int dir = Random.Range(0, 4);
                    switch (dir)
                    {
                        case 0: // NORTH
                            if (nodeGrid[(int)pos.x, (int)pos.y + 1] == null)
                            {
                                pos = new Vector2(pos.x, pos.y + 1);
                                node.GetRoom().GetComponent<Room>().SetEntrance(2, true);
                                node.GetParentNode().GetRoom().GetComponent<Room>().SetEntrance(0, true);
                                x = maxAttempts;
                                placed = true;
                            }
                            break;
                        case 1: // EAST
                            if (nodeGrid[(int)pos.x + 1, (int)pos.y] == null)
                            {
                                pos = new Vector2(pos.x + 1, pos.y);
                                node.GetRoom().GetComponent<Room>().SetEntrance(3, true);
                                node.GetParentNode().GetRoom().GetComponent<Room>().SetEntrance(1, true);
                                x = maxAttempts;
                                placed = true;
                            }
                            break;
                        case 2: // SOUTH
                            if (nodeGrid[(int)pos.x, (int)pos.y - 1] == null)
                            {
                                pos = new Vector2(pos.x, pos.y - 1);
                                node.GetRoom().GetComponent<Room>().SetEntrance(0, true);
                                node.GetParentNode().GetRoom().GetComponent<Room>().SetEntrance(2, true);
                                x = maxAttempts;
                                placed = true;
                            }
                            break;
                        case 3: // WEST
                            if (nodeGrid[(int)pos.x - 1, (int)pos.y] == null)
                            {
                                pos = new Vector2(pos.x - 1, pos.y);
                                node.GetRoom().GetComponent<Room>().SetEntrance(1, true);
                                node.GetParentNode().GetRoom().GetComponent<Room>().SetEntrance(3, true);
                                x = maxAttempts;
                                placed = true;
                            }
                            break;
                    }
                }
            }
             
            node.SetPosition(pos);
            nodeGrid[(int)pos.x, (int)pos.y] = node;

            if (!placed && node.GetNodeType() != RoomType.START)
            {
                Destroy(node.GetRoom());
                Debug.Log("ROOM MIS-PLACED");

            }
        }

        foreach(LevelNode node in nodeList)
        {
            node.GetRoom().transform.position = (node.GetPosition() - new Vector2(maxDepth,maxDepth)) * ROOMSIZE;
            node.GetRoom().GetComponent<Room>().Create();
        }

        

    }
}
