using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject laserPrefab;          // Prefab of the laser to shoot
    public float minShootInterval = 2f;     // Minimum interval between each shot
    public float maxShootInterval = 4f;     // Maximum interval between each shot
    private float timer;                    // Timer to track the shooting interval
    public Transform SpawnPoint;
     

    private void Start()
    {   
        timer = GetRandomShootInterval();    
    }
       

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ShootLaser();
             timer = GetRandomShootInterval();
        }
         
    }
    
    private void ShootLaser()
    {
        Instantiate(laserPrefab, SpawnPoint.position, Quaternion.Euler(0f, 0f, -90f));
    }

     
    private float GetRandomShootInterval()
    {
        return Random.Range(minShootInterval, maxShootInterval);
    }
}
