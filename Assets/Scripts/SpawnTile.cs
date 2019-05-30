using System;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
#pragma warning disable 0649 
    
    [SerializeField] private BoxCollider2D block;

    private void Start()
    {
        var tempBlock = Instantiate(block.gameObject, transform.position, Quaternion.identity);
        tempBlock.transform.SetParent(transform);
    }
}
