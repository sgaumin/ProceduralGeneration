using System;
using UnityEngine;

[Serializable]
public enum RoomTypes
{
    LR,
    LRB,
    LRT,
    LRTB,
}

public class Room : MonoBehaviour
{
    public RoomTypes roomType;
}