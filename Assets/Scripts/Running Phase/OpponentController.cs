using System.Collections;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    /*
     * 
     * Detects obstacles in front of AI. Moves opposite side of the obstacle.
     * 
     */

    private Rigidbody rb;
    private Runner runner;
    private Collider coll;
    private Animator animator;

    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private LayerMask platformLayerMask;

    private string obstacleLocation;
    private string ledgeLocation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        runner = GetComponent<Runner>();
        coll = GetComponent<Collider>();

        animator = GetComponent<Animator>();
        animator.SetFloat("runModifier", 1.2f);
    }

    private void Update()
    {
        ObstacleDetection();
        Debug.Log(LedgeControl() ? "Kenara Yaklasti" : "Kenarda Degil");
    }

    private void FixedUpdate()
    {
        if (runner._State == Runner.State.Running)
        {
            if (!LedgeControl())
            {
                switch (obstacleLocation)
                {
                    case "GoForward":
                        GoForward();
                        break;
                    case "GoLeft":
                        GoLeft();
                        break;
                    case "GoRight":
                        GoRight();
                        break;
                    case "RunForward":
                        GoForward(0.3f);
                        break;
                    case "RunLeft":
                        GoLeft(0.2f);
                        break;
                    case "RunRight":
                        GoRight(0.2f);
                        break;
                    case "Stop":
                        Stop();
                        break;
                }
            }
            else
            {
                if (ledgeLocation == "Left")
                {
                    GoRight(0.2f);
                }
                else if (ledgeLocation == "Right")
                {
                    GoLeft(0.2f);
                }
            }
            
        }
    }

    private void ObstacleDetection()
    {
        // We'll check opponents 0.4f front and detect if there's something or not.

        bool left, innerLeft, right, innerRight;

        Debug.DrawRay(coll.bounds.center, new Vector3(-0.3f, 0, 0.4f), Color.blue); // left
        Debug.DrawRay(coll.bounds.center, new Vector3(-0.1f, 0, 0.4f), Color.blue); // inner left
        Debug.DrawRay(coll.bounds.center, new Vector3(0.3f, 0, 0.4f), Color.blue); // right
        Debug.DrawRay(coll.bounds.center, new Vector3(0.1f, 0, 0.4f), Color.blue); // inner right

        right = Physics.Raycast(coll.bounds.center, new Vector3(0.3f, 0, 0.4f), coll.bounds.extents.z + 0.4f, obstacleLayerMask);
        innerRight = Physics.Raycast(coll.bounds.center, new Vector3(0.1f, 0, 0.4f), coll.bounds.extents.z + 0.4f, obstacleLayerMask);
        left = Physics.Raycast(coll.bounds.center, new Vector3(-0.3f, 0, 0.4f), coll.bounds.extents.z + 0.4f, obstacleLayerMask);
        innerLeft = Physics.Raycast(coll.bounds.center, new Vector3(-0.1f, 0, 0.4f), coll.bounds.extents.z + 0.4f, obstacleLayerMask);

        if (!left && !innerLeft && !right && !innerRight)
            obstacleLocation = "RunForward";
        else if (left && !innerLeft && !right && !innerRight)
            obstacleLocation = "RunRight";
        else if (!left && innerLeft && !right && !innerRight)
            obstacleLocation = "RunRight";
        else if (!left && !innerLeft && right && !innerRight)
            obstacleLocation = "RunLeft";
        else if (!left && !innerLeft && !right && innerRight)
            obstacleLocation = "RunLeft";
        else if (left && innerLeft && !right && !innerRight)
            obstacleLocation = "GoRight";
        else if (!left && !innerLeft && right && innerRight)
            obstacleLocation = "GoLeft";
        else if (left && innerLeft && !right && !innerRight)
            obstacleLocation = "GoRight";
        else if (left && !innerLeft && right && !innerRight)
            obstacleLocation = "GoForward";
        else if (!left && innerLeft && right && innerRight)
            obstacleLocation = "GoForward";
        else if (!left && innerLeft && !right && innerRight)
            obstacleLocation = "GoRight";
        else if (left && innerLeft && right && innerRight)
            obstacleLocation = "RunForward";
        else
            obstacleLocation = "There's a problem with Location.";
    }

    #region Movement Functions

    private void GoForward(float verticalSpeed = 0.25f)
    {
        rb.MovePosition(new Vector3(transform.position.x, rb.velocity.y, transform.position.z + verticalSpeed * Time.fixedDeltaTime));
    }
    private void GoLeft(float horizontalSpeed = 0.12f, float verticalSpeed = 0.25f)
    {
        rb.MovePosition(new Vector3(transform.position.x - horizontalSpeed * Time.fixedDeltaTime, rb.velocity.y, transform.position.z + verticalSpeed * Time.fixedDeltaTime));
    }
    private void GoRight( float horizontalSpeed = 0.12f, float verticalSpeed = 0.25f)
    {
        rb.MovePosition(new Vector3(transform.position.x + horizontalSpeed * Time.fixedDeltaTime, rb.velocity.y, transform.position.z + verticalSpeed * Time.fixedDeltaTime));
    }
    private void Stop()
    {
        rb.MovePosition(new Vector3(transform.position.x, rb.velocity.y, transform.position.z));
    }

    #endregion

    private bool LedgeControl()
    {
        bool left, right;

        Debug.DrawRay(coll.bounds.center, new Vector3(-0.05f, 0, 0), Color.green);
        Debug.DrawRay(coll.bounds.center, new Vector3(0.05f, 0, 0), Color.green);

        left = Physics.Raycast(coll.bounds.center, new Vector3(-0.05f, 0, 0), coll.bounds.extents.z + 0.05f, platformLayerMask, QueryTriggerInteraction.Collide);
        right = Physics.Raycast(coll.bounds.center, new Vector3(0.05f, 0, 0), coll.bounds.extents.z + 0.05f, platformLayerMask, QueryTriggerInteraction.Collide);

        if (left) ledgeLocation = "Left";
        if (right) ledgeLocation = "Right";
        
        if (left || right)
            return true;
        else
            return false;
    }
}

