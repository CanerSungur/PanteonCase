using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     * 
     * Detects which platform is game running and controls player accordingly.
     * Controls running animation's speed when vertical speed modifier changes.
     * 
     * # KEYBOARD GAMEPLAY # 
     * Space is for going forward,
     * A and D is for horizontal movement,
     * W is for going forward fast.
     * #####################
     * 
     */

    public bool isEnabled;

    private Rigidbody rb;
    private Runner player;

    private InputHandler inputHandler;

    private Animator animator;

    #if UNITY_STANDALONE_WIN

    private float horizontalMovement;
    private float verticalMovement;
    private float speed = 0.2f;

    #endif

    private void Start()
    {
        isEnabled = true;

        rb = GetComponent<Rigidbody>();
        player = GetComponent<Runner>();
        inputHandler = GameObject.Find("GameManager").GetComponent<InputHandler>();

        animator = GetComponent<Animator>();

        #if UNITY_STANDALONE_WIN
            animator.SetFloat("runModifier", 1f);  
        #endif
    }

    private void Update()
    {

        #if UNITY_IOS
            UpdateRunAnimationSpeed();
        #endif

        #if UNITY_ANDROID
            UpdateRunAnimationSpeed();
        #endif

        #region Keyboard Control

        #if UNITY_STANDALONE_WIN

    if (isEnabled)
            {

            if (Input.GetButton("Vertical"))
            {
                verticalMovement = Input.GetAxis("Vertical");
                if (player._State != Runner.State.Collided && player._State != Runner.State.Falling)
                {
                    player._State = Runner.State.Running;
                }
            }
            else
            {
                verticalMovement = 0;
                if (player._State != Runner.State.Collided && player._State != Runner.State.Falling)
                {
                    player._State = Runner.State.Idle;
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                speed = 0.35f;
                animator.SetFloat("runModifier", 1.5f);
            }
            else
            {
                speed = 0.2f;
                animator.SetFloat("runModifier", 1f);
            }

            if (Input.GetButton("Horizontal"))
                horizontalMovement = Input.GetAxis("Horizontal");
            else 
                horizontalMovement = 0;

            transform.Translate(horizontalMovement * speed * Time.deltaTime, 0, Mathf.Clamp(verticalMovement, 0, 2) * speed * Time.deltaTime, Space.World);

            // We set rotation 0 to avoid player's animation rotation change.
            player.transform.eulerAngles = new Vector3(0, 0, 0);
        }

#endif

        #endregion
    }

    private void FixedUpdate()
    {
        // Player's horizontal movement is on x axis, vertical movement is on z axis.
        if (player._State == Runner.State.Running)
        {
            if (inputHandler.GetVerticalDirection() == "Up") // We can only move horizontally if we are moving forward.
            {
                if (inputHandler.GetHorizontalDirection() == "Left")
                {
                    rb.MovePosition(new Vector3(transform.position.x - inputHandler.GetHorizontalSpeedModifier() * Time.fixedDeltaTime, rb.velocity.y, transform.position.z + inputHandler.GetVerticalSpeedModifier() * Time.fixedDeltaTime));
                }
                else if (inputHandler.GetHorizontalDirection() == "Right")
                {
                    rb.MovePosition(new Vector3(transform.position.x + inputHandler.GetHorizontalSpeedModifier() * Time.fixedDeltaTime, rb.velocity.y, transform.position.z + inputHandler.GetVerticalSpeedModifier() * Time.fixedDeltaTime));
                }
                else if (inputHandler.GetHorizontalDirection() == "None")
                {
                    rb.MovePosition(new Vector3(transform.position.x, rb.velocity.y, transform.position.z + inputHandler.GetVerticalSpeedModifier() * Time.fixedDeltaTime));
                }
            }
        }
    }

    private void UpdateRunAnimationSpeed()
    {
        animator.SetFloat("runModifier", inputHandler.GetVerticalSpeedModifier() * 4f);
    }
}
