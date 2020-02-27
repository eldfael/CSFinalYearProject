using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    RoomType roomType;
    Vector2 position;
    Vector2 size;
    LevelNode node;

    private void Start()
    {

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

}
