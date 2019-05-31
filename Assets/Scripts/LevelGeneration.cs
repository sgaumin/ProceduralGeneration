using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class LevelGeneration : MonoBehaviour
{
#pragma warning disable 0649 

    [SerializeField] private Transform[] startingTransform;
    [SerializeField] private Room[] rooms;
    [SerializeField] private LayerMask roomLayer;
    [SerializeField] private float timeBetweenRooms;
    [SerializeField] private float moveAmount;

    [SerializeField] private int minX;
    [SerializeField] private int maxX;
    [SerializeField] private int minY;

    private Directions _direction;
    private bool _endGeneration;
    private int _downMove = 0;

    private void Start()
    {
        StartCoroutine(LevelGenerator());
    }

    private IEnumerator LevelGenerator()
    {
        // Start
        var startingPositionIndex = Random.Range(0, startingTransform.Length);
        transform.position = startingTransform[startingPositionIndex].position;
        _direction = (Directions) Random.Range(0, 3);

        // Instantiate First room
        var roomIndex = Random.Range(0, rooms.Length);
        Instantiate(rooms[roomIndex], transform.position, Quaternion.identity);

        while (!_endGeneration)
        {
            yield return new WaitForSeconds(timeBetweenRooms);

            Move();
        }
    }

    private void Move()
    {
        var roomIndex = 0;


        switch (_direction)
        {
            case Directions.Right:
                if (transform.position.x < maxX)
                {
                    transform.position = new Vector2(transform.position.x + moveAmount, transform.position.y);

                    // Instantiate a room
                    roomIndex = Random.Range(0, rooms.Length);
                    Instantiate(rooms[roomIndex], transform.position, Quaternion.identity);

                    if (Random.Range(0f, 1f) >= 0.5f)
                        _direction = Directions.Down;
                    else
                        _direction = Directions.Right;

                    _downMove = 0;
                }
                else
                {
                    _direction = Directions.Down;
                }

                break;
            case Directions.Left:
                if (transform.position.x > minX)
                {
                    transform.position = new Vector2(transform.position.x - moveAmount, transform.position.y);

                    // Instantiate a room
                    roomIndex = Random.Range(0, rooms.Length);
                    Instantiate(rooms[roomIndex], transform.position, Quaternion.identity);

                    if (Random.Range(0f, 1f) >= 0.5f)
                        _direction = Directions.Down;
                    else
                        _direction = Directions.Left;

                    _downMove = 0;
                }
                else
                {
                    _direction = Directions.Down;
                }

                break;
            case Directions.Down:
                if (transform.position.y > minY)
                {
                    Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, roomLayer);
                    if (roomDetection.GetComponent<Room>().roomType != RoomTypes.LRB &&
                        roomDetection.GetComponent<Room>().roomType != RoomTypes.LRTB)
                    {
                        roomDetection.GetComponent<Room>().RoomDestruction();

                        roomIndex = Random.Range(0, rooms.Length);
                        if (_downMove == 0)
                        {
                            while (rooms[roomIndex].roomType != RoomTypes.LRB &&
                                   rooms[roomIndex].roomType != RoomTypes.LRTB)
                                roomIndex = Random.Range(0, rooms.Length);
                        }
                        else
                        {
                            while (rooms[roomIndex].roomType != RoomTypes.LRTB)
                                roomIndex = Random.Range(0, rooms.Length);
                        }

                        Instantiate(rooms[roomIndex], transform.position, Quaternion.identity);
                    }

                    transform.position = new Vector2(transform.position.x, transform.position.y - moveAmount);

                    // Instantiate a room
                    roomIndex = Random.Range(0, rooms.Length);
                    while (rooms[roomIndex].roomType != RoomTypes.LRT && rooms[roomIndex].roomType != RoomTypes.LRTB)
                        roomIndex = Random.Range(0, rooms.Length);

                    Instantiate(rooms[roomIndex], transform.position, Quaternion.identity);

                    _direction = (Directions) Random.Range(0, 3);
                    _downMove++;
                }
                else
                {
                    _endGeneration = true;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}