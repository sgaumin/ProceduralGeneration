using System;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    [SerializeField] private BoxCollider2D block;

    private void Start()
    {
        var tempBlock = Instantiate(block.gameObject, transform.position, Quaternion.identity);
        tempBlock.transform.SetParent(transform);
    }
}