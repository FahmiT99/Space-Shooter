using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class LaserController : MonoBehaviour
{
    public float speed = 10f;

    private void Update()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        
        // Check if the laser is off-screen
        if (viewportPos.x < 0f || viewportPos.x > 1f)
        {
            Destroy(gameObject);
        }
        Move();
    }
    private void Move()
    {
        Vector3 temp = transform.position;
        temp.x += speed * Time.deltaTime;
        transform.position = temp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBoss1"))
        {
            Destroy(gameObject);
        }
    }


}
