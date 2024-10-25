using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    public float value = 1f;
    public float moveSpeed = 8f;
    private Transform player;
    private bool isMovingToPlayer = false;
    private LootCollect lootCollect;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lootCollect = FindObjectOfType<LootCollect>();
    }

    private void Update()
    {
        if (isMovingToPlayer && player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            lootCollect.CollectLoot();
        }
    }

    public void MoveToPlayer()
    {
        isMovingToPlayer = true;
    }
}
