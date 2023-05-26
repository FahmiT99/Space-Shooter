using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;

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

    public GameObject TriplelaserPrefab;
    public Transform TriplelaserSpawnPoint;
    public AudioSource TriplelaserAudioSource;
    public float cooldownDuration = 1f;
    public Image cooldownIndicator;
    public Image blueImage;
    private float lastShotTime;
    private bool isCooldown;
    
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
            UpdateCooldownUI(cooldownProgress);

            if (timeSinceLastShot >= cooldownDuration)
            { 
                blueImage.GetComponent<Image>().enabled = true;
                isCooldown = false;
                
            }
        }
    }
    private void UpdateCooldownUI(float progress)
    {
        cooldownIndicator.fillAmount = 0f + progress;
    }
}
     
