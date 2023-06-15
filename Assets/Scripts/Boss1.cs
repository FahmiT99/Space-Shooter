using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;

public class Boss1Shooting : MonoBehaviour
{
    public GameObject laserPrefab;          // Prefab of the laser to shoot
    public float minShootInterval = 2f;     // Minimum interval between each shot
    public float maxShootInterval = 4f;     // Maximum interval between each shot
    private float timer;                    // Timer to track the shooting interval
    public Transform SpawnPoint;
    public Transform SpawnPoint2;            // Spawn point for shooting lasers


    private float fireRate;
    private float canFire = 3f;
    public Transform LaserSpawnPoint;
    [SerializeField] private GameObject enemyMegaLaserPrefab;
    public AudioSource MegaLaser;

    private void Start()
    {
        
        timer = GetRandomShootInterval();
        
    }
    private void Update()
    {
        ShootLaserPointer();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ShootLaser();
            timer = GetRandomShootInterval();
        }
    }
    private void ShootLaserPointer()
    {
        if(Time.time > canFire)
        {
            fireRate = Random.Range(3f, 7f);
            canFire = Time.time + fireRate;

            GameObject enemyMegaLaser = Instantiate(enemyMegaLaserPrefab, LaserSpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
            MegaLaser.pitch = 2f;
            MegaLaser.loop = true;
            MegaLaser.Play();
            enemyMegaLaser.transform.parent = transform;
            StartCoroutine(DestroyMegaLaser(enemyMegaLaser));
        }
    }
    private IEnumerator DestroyMegaLaser(GameObject enemyMegaLaser)
    {
        yield return new WaitForSeconds(1f);
        Destroy(enemyMegaLaser);
        MegaLaser.Stop();

    }
    private void ShootLaser()
    {
        Instantiate(laserPrefab, SpawnPoint.position, Quaternion.Euler(0f, 0f, -90f));    
        Instantiate(laserPrefab, SpawnPoint2.position, Quaternion.Euler(0f, 0f, -90f));
        
    }

    private float GetRandomShootInterval()
    {
        return Random.Range(minShootInterval, maxShootInterval);
    }
}
