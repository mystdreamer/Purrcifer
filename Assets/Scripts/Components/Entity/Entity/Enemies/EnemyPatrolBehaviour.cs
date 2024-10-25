using UnityEngine;

public class EnemyPatrolBehaviour : MonoBehaviour
{
    public float moveDistance = 10f;    // Distance to move in each direction
    public float speed = 2f;            // Movement speed

    private Vector3 startPos;
    private bool movingRight = true;

    private void Start()
    {
        startPos = transform.position;  // Set the starting position
    }

    private void Update()
    {
        // Calculate the target position based on the movement direction
        Vector3 targetPos = startPos + (movingRight ? Vector3.right : Vector3.left) * moveDistance;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Check if the GameObject has reached the target position
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            // Switch direction to repeat the movement
            movingRight = !movingRight;
        }
    }
}
