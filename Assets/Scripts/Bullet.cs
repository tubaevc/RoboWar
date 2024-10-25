using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject lootPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Drop();
            Destroy(other.gameObject);
        }
    }

    private void Drop()
    {
        Vector3 dropPos = transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        Instantiate(lootPrefab, dropPos, Quaternion.identity);
    }
}