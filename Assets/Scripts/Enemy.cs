using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Rendering;
using System.Reflection;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{

    [SerializeField] int enemyLife1 = 3;
    public ParticleSystem explosionParticleSystem;
    public ParticleSystem stopParticles1;
    public ParticleSystem stopParticles2;
    public float movementSpeed = 2f; // Speed of enemy movement
    private float minY; // Minimum Y position within the camera view
    private float maxY; // Maximum Y position within the camera view
    private int direction = 1;
    public AudioSource audioSource;
    public GameObject damageTextPrefab;
    private bool isMovementAllowed = true;
    private float stopDuration = 3f;
    private float stopTimer = 0f;

    private bool isCooldownActive = false;
    private float cooldownDuration = 20f;
    private float lastShotTime = 0f;
    private bool isParticlesActive = false;
    private GameObject electroImage;
    private GameObject cooldownIndicatorelectro;
    private Image ElectroImage;
    private Image cooldownIndicatorElectro;



    private void Start()
    {
        electroImage = GameObject.FindGameObjectWithTag("ElectroImage");
        cooldownIndicatorelectro = GameObject.FindGameObjectWithTag("ElectroCooldownIndicator");
        ElectroImage = electroImage.GetComponent<Image>();
        cooldownIndicatorElectro = cooldownIndicatorelectro.GetComponent<Image>();
        float halfEnemyHeight = transform.localScale.y / 2f;
        Camera mainCamera = Camera.main;
        // Calculate the minimum and maximum Y positions within the camera view
        minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + halfEnemyHeight;
        maxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - halfEnemyHeight;
    }
    void Update()
    {
        if (isMovementAllowed)
        {
            Vector3 newPosition = transform.position + new Vector3(0, direction * movementSpeed * Time.deltaTime, 0);

            // Check if the enemy has reached the boundary
            if (newPosition.y >= maxY || newPosition.y <= minY)
            {
                direction *= -1; // Reverse the movement direction
            }
            // Set the new position
            transform.position = newPosition;
        }
        else
        {
            // Stop the enemy's movement for a certain duration
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDuration)
            {
                isMovementAllowed = true;
                stopTimer = 0f;
                DeactivateParticles();
            }
        }
        if (isCooldownActive)
        {
            float timeSinceLastShot = Time.time - lastShotTime;
            float cooldownProgress = Mathf.Clamp01(timeSinceLastShot / cooldownDuration);
            UpdateCooldownUI(cooldownProgress, cooldownIndicatorElectro);

            if (timeSinceLastShot >= cooldownDuration)
            {
                ElectroImage.enabled = true;
                isCooldownActive = false;
            }             
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {

                ActivateParticles();
                lastShotTime = Time.time;
                isMovementAllowed = false;
                isCooldownActive = true;
                ElectroImage.enabled = false;
            }
        }
    }

    private void UpdateCooldownUI(float progress, Image cooldownIndicator)
    {
        cooldownIndicator.fillAmount = 0f + progress;
    }
    private void ActivateParticles()
    {
        if (!isParticlesActive)
        {
            if (stopParticles1 != null)
            {
                stopParticles1.Play();
            }

            if (stopParticles2 != null)
            {
                stopParticles2.Play();
            }

            isParticlesActive = true;
        }
    }

    private void DeactivateParticles()
    {
        if (isParticlesActive)
        {
            if (stopParticles1 != null)
            {
                stopParticles1.Stop();
            }

            if (stopParticles2 != null)
            {
                stopParticles2.Stop();
            }

            isParticlesActive = false;
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
    private void ShowFloatingDamageText(int damage)
    {
        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageText.GetComponent<TextMeshPro>().text = damage.ToString();
        // Adjust the position of the damage text above the enemy's sprite
        Vector3 textPosition = transform.position + new Vector3(0f, 0.5f, 0f);
        damageText.transform.position = textPosition;
        StartCoroutine(FadeOutText(damageText));
        Destroy(damageText.gameObject, 1f);
    }
    private IEnumerator FadeOutText(GameObject text)
    {
            Color startColor = text.GetComponent<TextMeshPro>().color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
            float fadeDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                if (text != null)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / fadeDuration);
                    text.GetComponent<TextMeshPro>().color = Color.Lerp(startColor, endColor, t);
                    yield return null;
                }
            }      
    }
    private void UpdateEnemyLife(int damage)
    {
        if (enemyLife1 > 0)
        {
            audioSource.Play();
            ShowFloatingDamageText(damage);
        }
        if (enemyLife1 <= 0)
        {
            ShowFloatingDamageText(damage);
            Destroy(gameObject);
            PlayExplosion();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedLaser"))
        {
            enemyLife1 -= 1;
            UpdateEnemyLife(1);
        }
        if (collision.gameObject.CompareTag("TripleLaser"))
        {
            enemyLife1 -= 2;
            UpdateEnemyLife(2);
            UpdateEnemyLife(2);
            UpdateEnemyLife(2);
        }
        if (collision.gameObject.CompareTag("Missile"))
        {
            enemyLife1 -= 5;
            UpdateEnemyLife(5);
        }
    }
}
