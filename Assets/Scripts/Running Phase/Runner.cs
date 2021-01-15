using System.Collections;
using UnityEngine;

public class Runner : MonoBehaviour
{
    /*
     * 
     * Checks collided and falling states, handles player's spawn to start and enables/disables control script.
     *
     */

    PlayerController playerController;

    private Transform[] startSpawnPoints;
    private bool canExecuteCoroutine = true;
    
    [HideInInspector] public State _State;
    public bool isThisAI;

    [HideInInspector] public int rank;
    [HideInInspector] public float zPosition;

    public enum State
    {
        Idle,
        Running,
        Falling,
        Collided,
        Victory
    }

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        startSpawnPoints = GameObject.Find("StartSpawnPoints").GetComponentsInChildren<Transform>();

        if (isThisAI)
            _State = State.Running;
        else
            _State = State.Idle;
    }

    private void Update()
    {

        zPosition = transform.position.z;

        #region Fall Check

        if (transform.position.y < -0.5f)
        {
            // We're falling.
            _State = State.Falling;

            // To ensure coroutine is executed just one time.
            if (canExecuteCoroutine)
            {
                canExecuteCoroutine = false;
                StartCoroutine(SpawnToStartPoint());
            }
        }

        #endregion
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // We hit something.
            _State = State.Collided;

            // To ensure coroutine is executed just one time.
            if (canExecuteCoroutine)
            {
                canExecuteCoroutine = false;
                StartCoroutine(SpawnToStartPoint());

                
            }
        }
    }

    public IEnumerator SpawnToStartPoint()
    {
        // We wait for animation to finish
        yield return new WaitForSeconds(1.5f);

        if (isThisAI)
            _State = State.Running;
        else
            _State = State.Idle;

        transform.position = startSpawnPoints[Random.Range(0, startSpawnPoints.Length)].position;

        canExecuteCoroutine = true;
    }
}
