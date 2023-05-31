using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

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
    public AudioSource Hit;
    public ParticleSystem explosionParticleSystem;

    //triple laser
    public GameObject TriplelaserPrefab;
    public Transform TriplelaserSpawnPoint;
    public AudioSource TriplelaserAudioSource;
    public float cooldownDuration = 1f;
    public Image cooldownIndicatorLaser;
    public Image blueImage;    
    private bool isCooldown;
    private float lastShotTime = 0f;

    //first special attack
    public GameObject missilePrefab;
    public float cooldownDuration1 = 5f;
    private bool isCooldownMissile;
    public Image cooldownIndicatorMissile;
    public Image MissileImage;
    public Transform missileSpawnPoint1;
    public Transform missileSpawnPoint2;
    private float lastShotTime2 = 0f;
    public AudioSource MissileAudioSource;


    // Shield Ability
    public GameObject ShieldPrefab;
    private bool shieldReady = true;
    private float cooldownDuration2 = 30f; // ShieldUsageTime + 20f
    private float shieldUsageTime = 10f;
    private float lastShieldActivationTime = 0f;

   
    //GOd mode
    public AudioSource GodModeAudioSource;
    public TextMeshProUGUI godModeIndicatorText;
    private bool isGodMode = false;



    private void Start()
    {
        ScreenHeight = Camera.main.orthographicSize;
        current_Shoot_timer = shoot_Timer;
        SetGodMode(false);
    }

    private void Update()
    {    
        MouseControlOption();
        KeyboardControlOption();
        GamepadControlOption();
        ShootLaser();
        TripleShot();
        ShootMissiles();
        ActivateShield();
        if (Input.GetKeyDown(KeyCode.G))
        {
            isGodMode = !isGodMode;
            SetGodMode(isGodMode);
        }

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
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Alpha1)) && !isCooldown)
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
         
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int numEnemies = enemies.Length;   

        if ((Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Alpha3)) && !isCooldownMissile)
        {
            if (numEnemies == 0)
            {
                GameObject missile1 = Instantiate(missilePrefab, missileSpawnPoint1.position, Quaternion.Euler(0f, 0f, -90f));
                GameObject missile2 = Instantiate(missilePrefab, missileSpawnPoint2.position, Quaternion.Euler(0f, 0f, -90f));
                MissileAudioSource.Play();
                lastShotTime2 = Time.time;
                isCooldownMissile = true;
                MissileImage.GetComponent<Image>().enabled = false;
            }
            if (numEnemies == 1)
            {
                GameObject targetEnemy = enemies[0];
                GameObject missile1 = Instantiate(missilePrefab, missileSpawnPoint1.position, Quaternion.Euler(0f, 0f, -90f));
                GameObject missile2 = Instantiate(missilePrefab, missileSpawnPoint2.position, Quaternion.Euler(0f, 0f, -90f));
                missile1.GetComponent<MissileController>().SetTarget(targetEnemy.transform);
                missile2.GetComponent<MissileController>().SetTarget(targetEnemy.transform);

                MissileAudioSource.Play();
                lastShotTime2 = Time.time;
                isCooldownMissile = true;
                MissileImage.GetComponent<Image>().enabled = false;
            }
            else if (enemies.Length > 1)
            {
                GameObject targetEnemy1 = GetRandomEnemy(enemies);
                GameObject targetEnemy2 = GetRandomEnemy(enemies, targetEnemy1);
                GameObject missile1 = Instantiate(missilePrefab, missileSpawnPoint1.position, Quaternion.Euler(0f, 0f, -45f));
                GameObject missile2 = Instantiate(missilePrefab, missileSpawnPoint2.position, Quaternion.Euler(0f, 0f, -90f));
                missile1.GetComponent<MissileController>().SetTarget(targetEnemy1.transform);
                missile2.GetComponent<MissileController>().SetTarget(targetEnemy2.transform);

                MissileAudioSource.Play();
                lastShotTime2 = Time.time;
                isCooldownMissile = true;
                MissileImage.GetComponent<Image>().enabled = false;
            }            
        }
        if (isCooldownMissile)
        {
            float timeSinceLastShot = Time.time - lastShotTime2;
            float cooldownProgress = Mathf.Clamp01(timeSinceLastShot / cooldownDuration);
            UpdateCooldownUI(cooldownProgress, cooldownIndicatorMissile);

            if (timeSinceLastShot >= cooldownDuration1)
            {
                MissileImage.GetComponent<Image>().enabled = true;
                isCooldownMissile = false;

            }
        }
    }

    private void SetGodMode(bool isActive)
    {
        // Enable/disable God Mode and update the indicator text
        isGodMode = isActive;

        if (isGodMode)
        {
            // Play the sound effect when God Mode is activated
            GodModeAudioSource.Play();

            // Show the God Mode indicator text
            godModeIndicatorText.text = "God Mode Activated";
        }
        else
        {
            // Hide the God Mode indicator text
            godModeIndicatorText.text = string.Empty;
        }
    }
    private void ActivateShield()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && shieldReady)
        {
            // Activate the shield as a child of the spaceship
            GameObject shield = Instantiate(ShieldPrefab, transform);
            shield.transform.localPosition = Vector3.zero;
            Destroy(shield, shieldUsageTime);

            shieldReady = false;
            lastShieldActivationTime = Time.time;

            StartCoroutine(ActivateShieldCooldown());
        }

        if (!shieldReady)
        {
            // Check if the cooldown is over
            float timeSinceLastActivation = Time.time - lastShieldActivationTime;
            if (timeSinceLastActivation >= cooldownDuration2)
            {
                shieldReady = true;
            }
        }
    }

    private IEnumerator ActivateShieldCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration2);
        shieldReady = true;

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

    
    private void OnCollisionEnter2D(Collision2D collision)
    {   if (!isGodMode)
        {
            if (collision.gameObject.CompareTag("EnemyLaser"))
            {
                if (explosionParticleSystem != null)
                {
                    ParticleSystem newExplosion = Instantiate(explosionParticleSystem, transform.position, Quaternion.identity);
                    newExplosion.Play();
                }
                Hit.Play();

                GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
                gameManager.StartFadeOut();
                gameManager.StartMainMenuDelay();
                Destroy(gameObject);

            }
        }
    }
    
}
     
