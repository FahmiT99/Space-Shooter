using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float speed = 9f;
    public Transform target;
    public float amplitude = 2f; // Amplitude of the wave
    public float frequency = 2f; // Frequency of the wave

    private float initialY; // Initial Y position of the missile
    private float randomOffset; // Random offset for movement

    private void Start()
    {
        initialY = transform.position.y;
        randomOffset = Random.Range(0f, 1f); // Generate a random offset for movement
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate the current position along the sine wave
            float yOffset = amplitude * Mathf.Sin((frequency * Time.time) + randomOffset);
            Vector3 targetPosition = target.transform.position + new Vector3(0f, yOffset, 0f);

            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            // No target, destroy the missile
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
             
            // Destroy the missile
            Destroy(gameObject);
        }
    }


}
