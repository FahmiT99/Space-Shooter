using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab of the laser to shoot
    public float minShootInterval = 2f; // Minimum interval between each shot
    public float maxShootInterval = 4f; // Maximum interval between each shot
    public float shootForce = 10f; // Force to apply to the laser

    private float timer; // Timer to track the shooting interval

    private void Start()
    {
        timer = GetRandomShootInterval();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Shoot();
            timer = GetRandomShootInterval();
        }
    }

    private void Shoot()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0f, 0f, -90f));
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();

        // Shoot the laser to the left
        laserRb.velocity = Vector2.left * shootForce;
    }

    private float GetRandomShootInterval()
    {
        return Random.Range(minShootInterval, maxShootInterval);
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("colided");
        }
    }
}

