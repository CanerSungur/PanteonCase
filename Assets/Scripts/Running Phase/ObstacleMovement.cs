using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("Obstacle Setup Field")]
    private Vector3 startingPosition;
    public Vector3 targetPosition;
    public float speed;
    private bool playerIsAtStartPos = true;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (playerIsAtStartPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.02f) playerIsAtStartPos = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startingPosition) < 0.02f) playerIsAtStartPos = true;
        }
    }
}
