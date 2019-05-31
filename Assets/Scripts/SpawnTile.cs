using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnTile : MonoBehaviour
{
#pragma warning disable 0649 

    [SerializeField] private GameObject[] block;

    private void Start()
    {
        var randIndex = Random.Range(0, block.Length);

        var tempBlock = Instantiate(block[randIndex], transform.position, Quaternion.identity);
        tempBlock.transform.SetParent(transform);
    }
}