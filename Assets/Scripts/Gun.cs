using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float baseFireRate = 10f;
    [SerializeField] private float fireRateMultiplier = 1f;
    private float nextFireTime = 0f;
    private const int POOL_SIZE = 30;
    private Queue<GameObject> bulletPool;

    public float FireRate => baseFireRate * fireRateMultiplier;

    private void Start()
    {
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    private GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        return null;
    }

    private void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
        }
        if(Input.GetKey(KeyCode.A))
        {
            BoostFireRate(2f);
        }
    }

    public void Fire()
    {
        nextFireTime = Time.time + (1f / FireRate);
        GameObject bullet = GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.SetActive(true);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * bulletSpeed;
            StartCoroutine(DisableBulletAfterDelay(bullet, 2f));
        }
    }

    private IEnumerator DisableBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBulletToPool(bullet);
    }

    public void BoostFireRate(float multiplier)
    {
        fireRateMultiplier = multiplier;
    }
}
