using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Vector2 levelSize;
    public Grid levelGrid;
    public LevelController(Vector2 _levelSize) 
    {
        levelSize = _levelSize;
    }

    public void Awake()
    {
        
        levelGrid = new Grid(levelSize, 1, transform);
    }

    public void OnDrawGizmos()
    {
        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                //Gizmos.DrawIcon(levelGrid.grid[x, y].levelPosition, "");
            }
        }
    }


}

public class Node 
{
    public bool walkable;
    public Vector2 levelPosition;

    public Node(bool _walkable, Vector2 _levelPosition) 
    {
        walkable = _walkable;
        levelPosition = _levelPosition;
    }
}

public class Grid
{
    public Vector2 levelSize;
    public float nodeSize;
    public Node[,] grid;

    public Grid(Vector2 _levelSize, float _nodeSize, Transform levelTransform)
    {
        levelSize = _levelSize;
        nodeSize = _nodeSize;
        Vector2 startingPoint = new Vector2(levelTransform.position.x - _levelSize.x/2, levelTransform.position.y - _levelSize.y/2);

        grid = new Node[Mathf.RoundToInt(levelSize.x / nodeSize), Mathf.RoundToInt(levelSize.y / nodeSize)];

        for (int x = 0; x < levelSize.x / nodeSize; x ++)
        {
            for (int y = 0; y < levelSize.y / nodeSize; y ++)
            {
                Vector2 levelPosition = new Vector2(startingPoint.x + x * nodeSize + (nodeSize / 2), startingPoint.y + y * nodeSize + (nodeSize / 2));
                bool walkable = !Physics2D.OverlapBox(levelPosition,new Vector2(nodeSize*0.95f,nodeSize*0.95f),0,LayerMask.GetMask("Wall"));

                Debug.Log(levelPosition);
                if (!walkable) { Debug.Log(walkable); }

                grid[x, y] = new Node(walkable, levelPosition);

            }
        }
    }

    
}