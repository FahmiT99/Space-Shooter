using UnityEngine;
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

    private void Start()
    {
        ScreenHeight = Camera.main.orthographicSize;
        current_Shoot_timer = shoot_Timer;
    }

    private void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        float newYPosition = Mathf.Clamp(transform.position.y + mouseY * moveSpeed * Time.deltaTime, -ScreenHeight +1, ScreenHeight -0.5f);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

        ShootLaser();
    }


    private void ShootLaser()
    {

        shoot_Timer += (Time.deltaTime   );
        if (shoot_Timer > current_Shoot_timer)
        {
            canShoot = true;
        }
        if ((Input.GetMouseButton(0)))
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
}
     
