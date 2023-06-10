using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject laserPrefab;                  // Prefab of the laser to shoot
    public GameObject circleLaserPrefab;            // Prefab of the circle laser object
    private float circleLaserMaxScale = 3f;          // Maximum scale of the circle laser
    public float circleLaserGrowDuration = 0.5f;      // Duration for the circle laser to grow
    public float circleLaserRotationSpeed = 360f;   // Speed of rotation for the circle laser
    public float minShootInterval = 2f;             // Minimum interval between each shot
    public float maxShootInterval = 4f;             // Maximum interval between each shot
    private float timer;                            // Timer to track the shooting interval
    public Transform laserSpawnPoint;               // Spawn point for shooting lasers
    public Transform circleSpawnPoint;              // Spawn point for the circle laser
    private Transform player;                       // Reference to the player's transform
    private Vector3 lastKnownPlayerPosition;        // Last known player position when shooting
    private GameObject circleLaserObject;           // Reference to the circle laser object

    private void Start()
    {
        timer = GetRandomShootInterval();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Instantiate the circle laser object as a child of the enemy
        circleLaserObject = Instantiate(circleLaserPrefab, circleSpawnPoint.position, Quaternion.identity);
        circleLaserObject.transform.parent = transform;
        circleLaserObject.SetActive(false);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            StartCircleLaserEffect();
            StartCoroutine(ShootAfterDelay(circleLaserGrowDuration));
            timer = GetRandomShootInterval();
        }
    }

    private void StartCircleLaserEffect()
    {
        circleLaserObject.SetActive(true);
        StartCoroutine(GrowCircleLaser());
    }

    private IEnumerator GrowCircleLaser()
    {
        float timer = 0f;
        float scale = 0f;

        while (timer < circleLaserGrowDuration)
        {
            timer += Time.deltaTime;
            scale = Mathf.Lerp(0f, circleLaserMaxScale, timer / circleLaserGrowDuration);
            circleLaserObject.transform.localScale = new Vector3(scale, scale, 1f);
            circleLaserObject.transform.Rotate(Vector3.forward, circleLaserRotationSpeed * Time.deltaTime);
            yield return null;
        }

        circleLaserObject.SetActive(false);
    }

    private IEnumerator ShootAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShootLaser();
    }
/*
    private void ShootLaser()
    {
        Vector3 direction = (lastKnownPlayerPosition - laserSpawnPoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        GameObject laser = Instantiate(laserPrefab, laserSpawnPoint.position, rotation);
        laser.GetComponent<Boss2Laser>().SetDirection(direction);
    }

    */


    private void ShootLaser()
    {
    Vector3 direction1 = Quaternion.Euler(0f, 0f, 10f) * Vector3.left;
    Vector3 direction2 = Quaternion.Euler(0f, 0f, 0f) * Vector3.left;
    Vector3 direction3 = Quaternion.Euler(0f, 0f, -10f) * Vector3.left;

    Quaternion rotation1 = Quaternion.LookRotation(Vector3.forward, direction1);
    Quaternion rotation2 = Quaternion.LookRotation(Vector3.forward, direction2);
    Quaternion rotation3 = Quaternion.LookRotation(Vector3.forward, direction3);

    GameObject laser1 = Instantiate(laserPrefab, laserSpawnPoint.position, rotation1);
    GameObject laser2 = Instantiate(laserPrefab, laserSpawnPoint.position, rotation2);
    GameObject laser3 = Instantiate(laserPrefab, laserSpawnPoint.position, rotation3);

    laser1.GetComponent<Boss2Laser>().SetDirection(direction1);
    laser2.GetComponent<Boss2Laser>().SetDirection(direction2);
    laser3.GetComponent<Boss2Laser>().SetDirection(direction3);
    }

    public void UpdateLastKnownPlayerPosition(Vector3 position)
    {
        lastKnownPlayerPosition = position;
    }

    private float GetRandomShootInterval()
    {
        return Random.Range(minShootInterval, maxShootInterval);
    }
}
