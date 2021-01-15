using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Run Phase Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject player;
    private Vector3 offset;

    [Header("Paint Phase Setup")]
    [SerializeField] private Camera secondCamera;

    private void Start()
    {
        offset = mainCamera.transform.position - player.transform.position;
    }

    void Update()
    {
        if (GameManager._Stage == GameManager.Stage.Running)
        {
            mainCamera.gameObject.SetActive(true);
            secondCamera.gameObject.SetActive(false);
            
            // Follow player.
            mainCamera.transform.position = player.transform.position + offset;
        }
        if (GameManager._Stage == GameManager.Stage.Painting)
        {
            mainCamera.gameObject.SetActive(false);
            secondCamera.gameObject.SetActive(true);
        }
    }
}
