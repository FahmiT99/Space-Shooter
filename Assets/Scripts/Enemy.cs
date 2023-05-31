using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections.Generic;
using System;

public class Enemy : MonoBehaviour
{

    [SerializeField] int enemyLife1 = 3;
    public ParticleSystem explosionParticleSystem;

    public float movementSpeed = 2f; // Speed of enemy movement
    private float minY; // Minimum Y position within the camera view
    private float maxY; // Maximum Y position within the camera view

    private int direction = 1;

    // Soundeffects:
    public AudioSource audioSource;

    //End of soundeffects

    private void Start()
    {
        float halfEnemyHeight = transform.localScale.y / 2f;
        Camera mainCamera = Camera.main;
        // Calculate the minimum and maximum Y positions within the camera view
        minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + halfEnemyHeight;
        maxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - halfEnemyHeight;
    }


    void Update()
    {
         Vector3 newPosition = transform.position + new Vector3(0, direction * movementSpeed * Time.deltaTime, 0);

        // Check if the enemy has reached the boundary
        if (newPosition.y >= maxY || newPosition.y <= minY)
        {
            direction *= -1; // Reverse the movement direction
        }

        // Set the new position
        transform.position = newPosition;

        if (enemyLife1 <= 0)
        {
            Destroy(gameObject);
            PlayExplosion();
        }
    }

    private void PlayExplosion()
    {
        if (explosionParticleSystem != null)
        {
            ParticleSystem newExplosion = Instantiate(explosionParticleSystem, transform.position, Quaternion.identity);
            newExplosion.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedLaser"))
        {
            enemyLife1 -= 1;
            if (enemyLife1 > 0)
            {
                audioSource.Play();
            }
        }

        if (collision.gameObject.CompareTag("TripleLaser"))
        {
            enemyLife1 -= 10;
            if (enemyLife1 > 0)
            {
                audioSource.Play();
            }
        }

        if (collision.gameObject.CompareTag("Missile"))
        {
            enemyLife1 -= 10;
            if (enemyLife1 > 0)
            {
                audioSource.Play();
            }
        }
    }


}
