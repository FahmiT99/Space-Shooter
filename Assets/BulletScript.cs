using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class BulletScript : MonoBehaviour
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
}
