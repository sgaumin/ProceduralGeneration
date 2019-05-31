using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomTypes roomType;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}