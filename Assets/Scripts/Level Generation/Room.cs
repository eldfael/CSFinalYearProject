﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType roomType;

    Vector2 position;
    Vector2 size;
    LevelNode node;
    public int depth;
    public bool[] entrances = new bool[4];
    bool active = false;
    bool completed = false;
    public int enemyCount = 0;

    public List<GameObject> layouts = new List<GameObject>();
    
    public List<GameObject> normalContents = new List<GameObject>();
    public List<GameObject> itemContents = new List<GameObject>();
    public List<GameObject> bossContents = new List<GameObject>();
    public List<GameObject> startContents = new List<GameObject>();

    GameObject gameController;

    public bool debug;

    private void Start()
    {
        if (debug)
        {
            roomType = RoomType.NORMAL;
            Create();
        }
        
        if (roomType == RoomType.START || roomType == RoomType.BOSS || roomType == RoomType.ITEM)
        {
            completed = true;
        }


        
    }

    public void Create()
    {
        gameController = GameObject.Find("Game Controller");

        // CREATE THE LAYOUT FOR THE ROOM
        int numOfEntrances = GetNumberOfEntrances();

        // IF 1 ENTRANCE
        if (numOfEntrances == 1)
        {
            // IF NORTH
            if (entrances[0])
            {
                Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_N"), transform);
            }
            // IF EAST
            if (entrances[1])
            {
                Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_E"), transform);
            }
            // IF SOUTH
            if (entrances[2])
            {
                Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_S"), transform);
            }
            // IF WEST
            if (entrances[3])
            {
                Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_W"), transform);
            }
        }
        // IF 2 ENTRANCES
        else if (numOfEntrances == 2)
        {
            // IF NORTH
            if (entrances[0])
            {
                // IF EAST
                if (entrances[1])
                {
                    Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_NE"), transform);
                }
                // IF SOUTH
                if (entrances[2])
                {
                    Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_NS"), transform);
                }
                // IF WEST
                if (entrances[3])
                {
                    Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_NW"), transform);
                }
            }
            // IF EAST
            else if (entrances[1])
            {
                // IF SOUTH
                if (entrances[2])
                {
                    Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_ES"), transform);
                }
                // IF WEST
                if (entrances[3])
                {
                    Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_EW"), transform);
                }
            }
            // IF NOT NORTH OR EAST MUST BE SOUTH AND WEST
            else
            {
                Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_SW"), transform);
            }
        }
        // IF 3 ENTRANCES
        else if (numOfEntrances == 3)
        {
            // IF NORTH
            if (entrances[0])
            {
                // IF EAST
                if (entrances[1])
                {
                    // IF SOUTH
                    if (entrances[2])
                    {
                        Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_NES"), transform);
                    }
                    // MUST BE WEST
                    else
                    {
                        Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_NEW"), transform);
                    }
                }
                // MUST BE SOUTH AND WEST
                else
                {
                    Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_NSW"), transform);
                }
            }
            // MUST BE ALL BUT NORTH
            else
            {
                Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_ESW"), transform);
            }
        }
        // MUST BE 4 ENTRANCES
        else
        {
            Instantiate(layouts.Find(GameObject => GameObject.name == "Layout_NESW"), transform);
        }
        
        // CREATE THE CONTENTS OF THE ROOM AFTER LAYOUT HAS BEEN CREATED
        switch(roomType)
        {
            case RoomType.NORMAL:
                Instantiate(normalContents[Random.Range(0, normalContents.Count)], transform);
                break;

            case RoomType.ITEM:   
                Instantiate(itemContents[Random.Range(0, itemContents.Count)], transform);
                break;

            case RoomType.START:
                if (gameController.GetComponent<GameController>().GetLevelPointer() == 0)
                {
                    Instantiate(startContents[0], transform);
                }
                else
                {
                    Instantiate(startContents[1], transform);
                }
                
                break;

            case RoomType.BOSS:
                Instantiate(bossContents[Random.Range(0, bossContents.Count)], transform);
                break;
        }


    }

    public void SetEntrance(int i, bool b)
    {
        entrances[i] = b;
    }
    public bool GetEntrance(int i)
    {
        return entrances[i];
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }
    public void SetRoomType(RoomType _roomType)
    {
        roomType = _roomType;
    }
    public void SetPosition(Vector2 _position)
    {
        position = _position;
    }
    public Vector2 GetPosition()
    {
        return position;
    }
    public void SetSize(Vector2 _size)
    {
        size = _size;
    }
    public Vector2 GetSize()
    {
        return size;
    }
    public void SetNode(LevelNode _node) 
    {
        node = _node;
    }
    public LevelNode GetNode()
    {
        return node;
    }

    public bool IsActive()
    {
        return active;
    }
    public bool IsCompleted()
    {
        return completed;
    }

    public void ActivateRoom()
    {
        active = true;
    }

    public void CompleteRoom()
    {
        completed = true;
    }

    public int GetNumberOfEntrances()
    {
        int numOfEntrances = 0;
        foreach (bool b in entrances)
        {
            if (b)
            {
                numOfEntrances += 1;
            }
        }
        return numOfEntrances;
    }

}
