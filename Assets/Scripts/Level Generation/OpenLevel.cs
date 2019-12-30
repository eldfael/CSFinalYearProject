using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevel : MonoBehaviour
{

    int nodeSize = 2;
    
    int tailQuantity = 3;
    int tailSize = 30;
    
    int roomCount = 10;
    int roomMaxSize = 4;
    int roomMinSize = 2;
    
    int growQuantity = 10;
    float growChance = 0.2f;

    public GameObject wallObject; // Put in gameobject
    public GameObject floorObject;

    Node[,] nodeArray;

    List<Vector2> emptyNodePositions = new List<Vector2>();
    List<Node> toGrow = new List<Node>();
    List<Node> nodeList = new List<Node>();

    int arraySize;
    int gLimit;
    List<Node> nextList;
    

    Node startNode;

    private void Start()
    {
        // Initialize array with appropriate size
        arraySize = (2 * tailSize) + (2 * growQuantity) + 3;
        nodeArray = new Node[arraySize,arraySize];
        // Fill array with empty nodes
        for (int x = 0; x < arraySize; x++)
        { 
            for (int y = 0; y < arraySize; y++)
            {
                nodeArray[x, y] = new Node(NodeType.EMPTY, new Vector2(x, y));
            }
        }
        // Set the centre of the array to be the starting node
        startNode = nodeArray[tailSize + growQuantity + 1, tailSize + growQuantity + 1] = new Node(NodeType.OPEN, new Vector2(tailSize + growQuantity + 1, tailSize + growQuantity + 1));
        toGrow.Add(startNode);

        // Create tail(s)
        Vector2 nextPosition;
        for (int c = 0; c < tailQuantity; c++)
        {
            Node currentNode = startNode;

            for (int t = 0; t < tailSize; t++)
            {
                emptyNodePositions = GetEmptyNodes(currentNode.GetPosition());
                if (emptyNodePositions.Count > 0)
                {
                    nextPosition = emptyNodePositions[Random.Range(0, emptyNodePositions.Count - 1)];
                    nodeArray[(int)nextPosition.x, (int)nextPosition.y] = new Node(currentNode, NodeType.OPEN, nextPosition);
                    currentNode = nodeArray[(int)nextPosition.x, (int)nextPosition.y];
                    
                    toGrow.Add(currentNode);
                    nodeList.Add(currentNode);
                }
                else 
                {
                    if (currentNode.GetCreator() != null)
                    {
                        t -= 1;
                        currentNode = currentNode.GetCreator();
                    }
                    else
                    { 
                        // This means the starting node has no adjacent empty nodes ..
                    }
                }
            }
        }

        
        for (int grow = 0; grow < growQuantity; grow ++)
        {
            gLimit = toGrow.Count;
            nextList = new List<Node>();

            for (int g = 0; g < gLimit; g++)
            {
                emptyNodePositions = GetEmptyNodes(toGrow[g].GetPosition());
                for (int e = 0; e < emptyNodePositions.Count; e++)
                {
                    if (Random.Range(0, 1f) < growChance)
                    {
                        nodeArray[(int)emptyNodePositions[e].x, (int)emptyNodePositions[e].y] = new Node(toGrow[g], NodeType.OPEN, emptyNodePositions[e]);
                        nextList.Add(nodeArray[(int)emptyNodePositions[e].x, (int)emptyNodePositions[e].y]);
                        nodeList.Add(nodeArray[(int)emptyNodePositions[e].x, (int)emptyNodePositions[e].y]);
                    }
                    else
                    {
                        nodeArray[(int)emptyNodePositions[e].x, (int)emptyNodePositions[e].y] = new Node(toGrow[g], NodeType.WALL, emptyNodePositions[e]);
                        nodeList.Add(nodeArray[(int)emptyNodePositions[e].x, (int)emptyNodePositions[e].y]);
                    }
                }
            }
            toGrow = nextList;
        }

        /*
        for (int room = 0; room < roomCount; room++)
        {
            int nextRoom = Random.Range(0, toGrow.Count - 1);
            
            int xSize = Random.Range(roomMinSize, roomMaxSize);
            int xDir = Random.Range(0, 1) * 2 - 1;

            int ySize = Random.Range(roomMinSize, roomMaxSize);
            int yDir = Random.Range(0, 1) * 2 - 1;

            bool canPlace = true;

            for (int x = -xDir ; Mathf.Sign(x) * x < xSize + 1; x += xDir)
            {
                for (int y = -yDir; Mathf.Sign(y) * y < ySize + 1; y += yDir)
                {
                    if (nodeArray[(int)toGrow[nextRoom].GetPosition().x + x, (int)toGrow[nextRoom].GetPosition().y + y].GetNodeType() == NodeType.ROOM)
                    {
                        canPlace = false;
                    }
                }
            }

            if (canPlace)
            {
                for (int x = 0; Mathf.Sign(x) * x < xSize; x += xDir)
                {
                    for (int y = 0; Mathf.Sign(y) * y < ySize; y += yDir)
                    {
                        nodeArray[(int)toGrow[nextRoom].GetPosition().x + x, (int)toGrow[nextRoom].GetPosition().y + y] = new Node(NodeType.ROOM, new Vector2((int)toGrow[nextRoom].GetPosition().x + x, (int)toGrow[nextRoom].GetPosition().y + y));
                        nodeList.Add(nodeArray[(int)toGrow[nextRoom].GetPosition().x + x, (int)toGrow[nextRoom].GetPosition().y + y]);
                    }
                }
                toGrow.Remove(toGrow[nextRoom]);
            }
            else
            {
                room--;
            }
        }
        */

        for (int w = 0; w < 2; w++)
        {
            gLimit = nodeList.Count;
            nextList = new List<Node>();

            for (int g = 0; g < gLimit; g++)
            {
                emptyNodePositions = GetEmptyNodes(nodeList[g].GetPosition());
                for (int e = 0; e < emptyNodePositions.Count; e++)
                {
                    nodeArray[(int)emptyNodePositions[e].x, (int)emptyNodePositions[e].y] = new Node(nodeList[g], NodeType.WALL, emptyNodePositions[e]);
                    nextList.Add(nodeArray[(int)emptyNodePositions[e].x, (int)emptyNodePositions[e].y]);
                }
            }
            nodeList = nextList;
        }

        

        for (int x = 0; x < arraySize; x++)
        {
            for (int y = 0; y < arraySize; y++)
            {
                if (nodeArray[x, y].GetNodeType() == NodeType.WALL)
                {
                    FillObject(nodeArray[x, y].GetPosition(),wallObject);
                }
            }
        }

        for (int x = 0; x < arraySize; x++)
        {
            for (int y = 0; y < arraySize; y++)
            {
                if (nodeArray[x, y].GetNodeType() == NodeType.OPEN || nodeArray[x, y].GetNodeType() == NodeType.GROWN)
                {
                    FillObject(nodeArray[x, y].GetPosition(), floorObject);
                }
            }
        }

    }

    /*
    private void OnDrawGizmos()
    {
        for (int x = 0; x < arraySize; x++)
        {
            for (int y = 0; y < arraySize; y++)
            {
                if (nodeArray[x, y].GetNodeType() == NodeType.EMPTY) { Gizmos.color = Color.green; }
                else if (nodeArray[x, y].GetNodeType() == NodeType.OPEN) { Gizmos.color = Color.blue; }
                else if (nodeArray[x, y].GetNodeType() == NodeType.WALL) { Gizmos.color = Color.red; }
                else if (nodeArray[x, y].GetNodeType() == NodeType.ROOM) { Gizmos.color = Color.cyan; }

                Gizmos.DrawCube(nodeArray[x,y].GetPosition(),new Vector3(0.5f,0.5f));

            }
        }
    }
    */

    private List<Vector2> GetEmptyNodes(Vector2 position) 
    {
        List<Vector2> emptyNodes = new List<Vector2>();

        for (int x = -1; x < 2; x += 2)
        {
            if (nodeArray[(int)position.x + x, (int)position.y].GetNodeType() == NodeType.EMPTY)
            {
                emptyNodes.Add(new Vector2(position.x + x, position.y));
            }
        }
        for (int y = -1; y < 2; y += 2)
        {
            if (nodeArray[(int)position.x, (int)position.y + y].GetNodeType() == NodeType.EMPTY)
            {
                emptyNodes.Add(new Vector2(position.x, position.y + y));
            }
        }
        
        return emptyNodes;
    }

    private void FillObject(Vector2 position, GameObject obj)
    {
        for (int x = 0; x < nodeSize; x++)
        {
            for (int y = 0; y < nodeSize; y++)
            {
                Instantiate(obj, new Vector2(position.x*nodeSize+x,position.y*nodeSize+y), Quaternion.identity);
            }
        }
    }

}
