using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject laserPrefab;          // Prefab of the laser to shoot
    public GameObject throwableObjectPrefab; // Prefab of the object to throw
    public float minShootInterval = 2f;     // Minimum interval between each shot
    public float maxShootInterval = 4f;     // Maximum interval between each shot
    private float timer;                    // Timer to track the shooting interval
    public Transform SpawnPoint;
    public Transform SpawnPoint2;            // Spawn point for shooting lasers
    public Transform throwSpawnPoint;       // Spawn point for throwing objects
    public float throwSpeed = 5f;           // Speed of the thrown objects
    public float throwReverseSpeed = 2f;    // Speed of the thrown objects when reversing
    public float throwBoundaryOffset = 1f;  // Offset from camera boundary to reverse the thrown objects
    public int throwObjectAmount = 5;       // Amount of objects to throw in a row
    private bool isThrowing = false;        // Flag to indicate if currently throwing objects

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
            if (gameObject.CompareTag("EnemyBoss1"))
            {
                ThrowObjects();
            }
            timer = GetRandomShootInterval();
        }

        if (isThrowing)
        {
            MoveThrownObjects();
        }
    }

    private void ShootLaser()
    {
        Instantiate(laserPrefab, SpawnPoint.position, Quaternion.Euler(0f, 0f, -90f));
        if (gameObject.CompareTag("EnemyBoss1"))
        {
            Instantiate(laserPrefab, SpawnPoint2.position, Quaternion.Euler(0f, 0f, -90f));
        }
    }

    private void ThrowObjects()
    {
        isThrowing = true;

        for (int i = 0; i < throwObjectAmount; i++)
        {
            GameObject thrownObject = Instantiate(throwableObjectPrefab, throwSpawnPoint.position, Quaternion.Euler(0f, 0f, -90f));
            Rigidbody2D thrownRigidbody = thrownObject.GetComponent<Rigidbody2D>();
            thrownRigidbody.velocity = Vector2.up * throwSpeed;
        }
    }

    private void MoveThrownObjects()
    {
        GameObject[] thrownObjects = GameObject.FindGameObjectsWithTag("ThrownObject");

        foreach (GameObject thrownObject in thrownObjects)
        {
            Rigidbody2D thrownRigidbody = thrownObject.GetComponent<Rigidbody2D>();

            // Check if the thrown object is reaching the camera boundary
            if (thrownObject.transform.position.y >= Camera.main.ViewportToWorldPoint(Vector3.up).y - throwBoundaryOffset)
            {
                // Reverse the direction by changing the velocity
                thrownRigidbody.velocity = Vector2.down * throwReverseSpeed;
            }
            if (thrownObject.transform.position.y <= Camera.main.ViewportToWorldPoint(Vector3.up).y + throwBoundaryOffset)
            {
                // Reverse the direction by changing the velocity
                thrownRigidbody.velocity = Vector2.down * throwReverseSpeed;
            }
        }
    }

    private float GetRandomShootInterval()
    {
        return Random.Range(minShootInterval, maxShootInterval);
    }
}
