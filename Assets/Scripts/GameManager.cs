using UnityEngine;

public class GameManager : MonoBehaviour
{ /*
   * 
   * What does this script do ?
   *  
   */

    [Header("Run Phase Scripts")]
    private PlayerController playerController;
    private Runner player;

    [Header("Paint Phase Scripts")]
    [SerializeField] private TexturePainter texturePainter;
    [SerializeField] private SprayController sprayController;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject sprayCan;

    public static Stage _Stage;

    public enum Stage
    {
        Running,
        Painting
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Runner>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.enabled = true;

        _Stage = Stage.Running;
    }

    private void Update()
    {
        if (_Stage == Stage.Running)
        {
            if (player._State == Runner.State.Collided || player._State == Runner.State.Falling)
            {
                playerController.isEnabled = false;
            }
            else if (player._State == Runner.State.Idle)
            {
                // To avoid player slide left and right after fall state.
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                playerController.isEnabled = true;
            }
            else
            {
                playerController.isEnabled = true;
            }
                

            sprayController.enabled = false;
            texturePainter.enabled = false;
            wall.SetActive(false);
            sprayCan.SetActive(false);
        }
        else if (_Stage == Stage.Painting)
        {
            playerController.isEnabled = false;
            sprayController.enabled = true;
            texturePainter.enabled = true;
            wall.SetActive(true);
            sprayCan.SetActive(true);
        }
    }
}
