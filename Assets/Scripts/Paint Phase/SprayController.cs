using UnityEngine;

public class SprayController : MonoBehaviour
{
    [Header("Touch Movement Setup")]
    private Rigidbody rb;
    private Spray spray;
    private InputHandler inputHandler;

    [Header("Keyboard Movement Setup")]
    #if UNITY_STANDALONE_WIN
    private float speed = 0.2f;
    private float horizontalMovement = 0;
    private float verticalMovement = 0;
    #endif


    [SerializeField] private ParticleSystem sprayParticleEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        spray = GetComponent<Spray>();

        inputHandler = GameObject.Find("GameManager").GetComponent<InputHandler>();
    }

    private void Update()
    {

        #region Keyboard Input & Movement

        #if UNITY_STANDALONE_WIN

                if (Input.GetKey(KeyCode.Space))
                    spray._State = Spray.State.Painting;
                else
                    spray._State = Spray.State.Idle;

                if (Input.GetButton("Horizontal"))
                    horizontalMovement = Input.GetAxis("Horizontal");
                else
                    horizontalMovement = 0;
                if (Input.GetButton("Vertical"))
                    verticalMovement = Input.GetAxis("Vertical");
                else
                    verticalMovement = 0;

                transform.Translate(horizontalMovement * speed * Time.deltaTime, verticalMovement * speed * Time.deltaTime, 0, Space.World);

        #endif


        #endregion

        if (spray._State == Spray.State.Idle)
        {
            sprayParticleEffect.Stop();
        }
        else if (spray._State == Spray.State.Painting)
            sprayParticleEffect.Play();

    }

    private void FixedUpdate()
    {
        // Spray's horizontal movement is on x axis, vertical movement is on y axis.

        if (spray._State == Spray.State.Painting)
        {
            rb.MovePosition(new Vector3(transform.position.x + inputHandler.GetTouchDeltaPosition().x * inputHandler.GetVerticalSpeedModifier() * Time.fixedDeltaTime, transform.position.y + inputHandler.GetTouchDeltaPosition().y * inputHandler.GetVerticalSpeedModifier() * Time.fixedDeltaTime, transform.position.z));
        }
    }
}
