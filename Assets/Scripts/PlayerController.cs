using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public AudioSource laserAudioSource;
    public float shoot_Timer = 0.1f;
    private float current_Shoot_timer;
    private bool canShoot;
    private float ScreenHeight;
    public float sensitivity = 7f;
    //triple laser
    public GameObject TriplelaserPrefab;
    public Transform TriplelaserSpawnPoint;
    public AudioSource TriplelaserAudioSource;
    public float cooldownDuration = 1f;
    public Image cooldownIndicatorLaser;
    public Image blueImage;
    
    private bool isCooldown;
    //first special attack
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public float cooldownDuration1 = 5f;
    private bool isCooldownMissile;
    public Image cooldownIndicatorMissile;
    public Transform missileSpawnPoint1;
    public Transform missileSpawnPoint2;

    private void Start()
    {
        ScreenHeight = Camera.main.orthographicSize;
        current_Shoot_timer = shoot_Timer;
    }

    private void Update()
    {    
        MouseControlOption();
        KeyboardControlOption();
        GamepadControlOption();
        ShootLaser();
        TripleShot();
        ShootMissiles();

    }

    private void MouseControlOption()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        float newYPosition = Mathf.Clamp(transform.position.y + mouseY * moveSpeed * Time.deltaTime, -ScreenHeight +1, ScreenHeight -0.5f);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }
    private void KeyboardControlOption()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float newYPosition = transform.position.y + verticalInput * moveSpeed * Time.deltaTime;
        float clampedYPosition = Mathf.Clamp(newYPosition, -ScreenHeight + 1, ScreenHeight - 0.5f);
        transform.position = new Vector3(transform.position.x, clampedYPosition, transform.position.z);
    }

    private void GamepadControlOption()
    {
        float gamepadY = Input.GetAxis("Vertical");
        if (Mathf.Abs(gamepadY) > 0.1f)
        {
            float newYPosition = transform.position.y + gamepadY * moveSpeed * Time.deltaTime;
            float clampedYPosition = Mathf.Clamp(newYPosition, -ScreenHeight + 1, ScreenHeight - 0.5f);
            transform.position = new Vector3(transform.position.x, clampedYPosition, transform.position.z);
        }
    }



    private void ShootLaser()
    {

        shoot_Timer += (Time.deltaTime   );
        if (shoot_Timer > current_Shoot_timer)
        {
            canShoot = true;
        }
        if ((Input.GetMouseButton(0)) || Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1"))
        {
            if (canShoot)
            {
                canShoot = false;
                shoot_Timer = 0f;
                Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.Euler(0f, 0f, 90f));
                laserAudioSource.Play();
            }
        }
    }

    private void TripleShot()
    {
         float lastShotTime = 0f;

        if (Input.GetMouseButtonDown(1) && !isCooldown)
        {
            Instantiate(TriplelaserPrefab, TriplelaserSpawnPoint.position, transform.rotation);
            TriplelaserAudioSource.Play();
            lastShotTime = Time.time;
            isCooldown = true;
            blueImage.GetComponent<Image>().enabled = false;
        }

        if (isCooldown)
        {
            float timeSinceLastShot = Time.time - lastShotTime;
            float cooldownProgress = Mathf.Clamp01(timeSinceLastShot / cooldownDuration);
            UpdateCooldownUI(cooldownProgress, cooldownIndicatorLaser);

            if (timeSinceLastShot >= cooldownDuration)
            { 
                blueImage.GetComponent<Image>().enabled = true;
                isCooldown = false;
                
            }
        }
    }
    private void ShootMissiles()
    {
        float lastShotTime = 0f;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int numEnemies = enemies.Length;

        if (numEnemies == 0)
        {
            // No enemies, return
            return;
        }

        if (Input.GetMouseButtonDown(1) && !isCooldownMissile)
        {
            if (numEnemies == 1)
            {
                // Only one enemy, both missiles target it
                GameObject targetEnemy = enemies[0];
                GameObject missile1 = Instantiate(missilePrefab, missileSpawnPoint1.position, Quaternion.Euler(0f, 0f, -90f));
                GameObject missile2 = Instantiate(missilePrefab, missileSpawnPoint2.position, Quaternion.Euler(0f, 0f, -90f));

                missile1.GetComponent<MissileController>().SetTarget(targetEnemy.transform);
                missile2.GetComponent<MissileController>().SetTarget(targetEnemy.transform);

                //TriplelaserAudioSource.Play();
                lastShotTime = Time.time;
                isCooldownMissile = true;
            }
            else if (enemies.Length > 1)
            {
                // Multiple enemies, each missile targets a random enemy
                GameObject targetEnemy1 = GetRandomEnemy(enemies);
                GameObject targetEnemy2 = GetRandomEnemy(enemies, targetEnemy1);

                GameObject missile1 = Instantiate(missilePrefab, missileSpawnPoint1.position, Quaternion.identity);
                GameObject missile2 = Instantiate(missilePrefab, missileSpawnPoint2.position, Quaternion.identity);

                missile1.GetComponent<MissileController>().SetTarget(targetEnemy1.transform);
                missile2.GetComponent<MissileController>().SetTarget(targetEnemy2.transform);
                //TriplelaserAudioSource.Play();
                lastShotTime = Time.time;
                 isCooldownMissile = true;
            }            
        }
        if (isCooldownMissile)
        {
            float timeSinceLastShot = Time.time - lastShotTime;
            float cooldownProgress = Mathf.Clamp01(timeSinceLastShot / cooldownDuration);
            UpdateCooldownUI(cooldownProgress, cooldownIndicatorMissile);

            if (timeSinceLastShot >= cooldownDuration1)
            {
                blueImage.GetComponent<Image>().enabled = true;
                isCooldownMissile = false;

            }
        }
    }

    private GameObject GetRandomEnemy(GameObject[] enemies, GameObject ignore = null)
    {
        List<GameObject> availableEnemies = new List<GameObject>(enemies);

        if (ignore != null)
        {
            availableEnemies.Remove(ignore);
        }

        int randomIndex = Random.Range(0, availableEnemies.Count);
        return availableEnemies[randomIndex];
    }
    private void UpdateCooldownUI(float progress, Image cooldownIndicator)
    {
        cooldownIndicator.fillAmount = 0f + progress;
    }
}
     
