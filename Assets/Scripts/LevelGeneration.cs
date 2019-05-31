using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class LevelGeneration : MonoBehaviour
{
#pragma warning disable 0649 

    [Header("Spawning Parameters")] [SerializeField]
    private LayerMask roomLayer;

    [SerializeField] private float timeBetweenRooms;

    [Header("Grid Parameters")] [SerializeField]
    private int minX;

    [SerializeField] private int maxX;
    [SerializeField] private int minY;
    [SerializeField] private float moveAmount;

    [Header("Elements")] [SerializeField] private Transform[] startingTransform;
    [SerializeField] private Room[] roomTemplatesMainPath;
    [SerializeField] private Room[] roomTemplatesOthers;

    private Directions _direction;
    private Spawn[] _spawns;
    private bool _endGenerationMainPath;
    private int _downMove = 0;

    private void Start()
    {
        _spawns = FindObjectsOfType<Spawn>();

        StartCoroutine(LevelGenerator());
    }

    private IEnumerator LevelGenerator()
    {
        // ---- Main Path Generation ----

        // Start
        var startingPositionIndex = Random.Range(0, startingTransform.Length);
        transform.position = startingTransform[startingPositionIndex].position;
        _direction = (Directions) Random.Range(0, 3);

        // Instantiate First room
        var roomIndex = Random.Range(0, roomTemplatesMainPath.Length);
        Instantiate(roomTemplatesMainPath[roomIndex], transform.position, Quaternion.identity);

        while (!_endGenerationMainPath)
        {
            yield return new WaitForSeconds(timeBetweenRooms);

            Move();
        }

        // ---- Other Rooms Generation ----
        foreach (var spawn in _spawns)
        {
            var roomDetection = Physics2D.OverlapCircle(spawn.transform.position, 1, roomLayer);
            if (roomDetection == null)
            {
                roomIndex = Random.Range(0, roomTemplatesOthers.Length);
                Instantiate(roomTemplatesOthers[roomIndex], spawn.transform.position, Quaternion.identity);
            }
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
                    roomIndex = Random.Range(0, roomTemplatesMainPath.Length);
                    Instantiate(roomTemplatesMainPath[roomIndex], transform.position, Quaternion.identity);

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
                    roomIndex = Random.Range(0, roomTemplatesMainPath.Length);
                    Instantiate(roomTemplatesMainPath[roomIndex], transform.position, Quaternion.identity);

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

                        roomIndex = Random.Range(0, roomTemplatesMainPath.Length);
                        if (_downMove == 0)
                        {
                            while (roomTemplatesMainPath[roomIndex].roomType != RoomTypes.LRB &&
                                   roomTemplatesMainPath[roomIndex].roomType != RoomTypes.LRTB)
                                roomIndex = Random.Range(0, roomTemplatesMainPath.Length);
                        }
                        else
                        {
                            while (roomTemplatesMainPath[roomIndex].roomType != RoomTypes.LRTB)
                                roomIndex = Random.Range(0, roomTemplatesMainPath.Length);
                        }

                        Instantiate(roomTemplatesMainPath[roomIndex], transform.position, Quaternion.identity);
                    }

                    transform.position = new Vector2(transform.position.x, transform.position.y - moveAmount);

                    // Instantiate a room
                    roomIndex = Random.Range(0, roomTemplatesMainPath.Length);
                    while (roomTemplatesMainPath[roomIndex].roomType != RoomTypes.LRT &&
                           roomTemplatesMainPath[roomIndex].roomType != RoomTypes.LRTB)
                        roomIndex = Random.Range(0, roomTemplatesMainPath.Length);

                    Instantiate(roomTemplatesMainPath[roomIndex], transform.position, Quaternion.identity);

                    _direction = (Directions) Random.Range(0, 3);
                    _downMove++;
                }
                else
                {
                    _endGenerationMainPath = true;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}