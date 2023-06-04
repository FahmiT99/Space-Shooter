using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float speed = 9f;
    public Transform target;
    public float amplitude = 2f; // Amplitude of the wave
    public float frequency = 2f; // Frequency of the wave
    public float rotateSpeed = 200f;


    private void Update()
    {
        if (target != null)
        {         
            Vector3 direction = (target.position - transform.position).normalized;

            // Rotate the missile towards the target
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            // Move the missile towards the target
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            Vector3 temp = transform.position;
            temp.x += speed * Time.deltaTime;
            transform.position = temp;
        }
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPos.x < 0f || viewportPos.x > 1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBoss1"))
        {
            Destroy(gameObject);
        }
    }


}
