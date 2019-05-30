using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Directions
{
    Right,
    Left,
    Down
}

public class LevelGeneration : MonoBehaviour
{
#pragma warning disable 0649 
    
    [SerializeField] private Transform[] startingTransform;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private float timeBetweenRooms;
    [SerializeField] private float moveAmount;

    [SerializeField] private int minX;
    [SerializeField] private int maxX;
    [SerializeField] private int minY;

    private bool _endGeneration;
    private Directions _direction;

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
        int roomIndex;
        
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
                }
                else
                {
                    _direction = Directions.Down;
                }

                break;
            case Directions.Down:
                if (transform.position.y > minY)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - moveAmount);
                    
                    // Instantiate a room
                    roomIndex = Random.Range(0, rooms.Length);
                    Instantiate(rooms[roomIndex], transform.position, Quaternion.identity);
                    
                    _direction = (Directions) Random.Range(0, 3);
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