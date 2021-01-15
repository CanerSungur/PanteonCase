using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text infoText;
    public GameObject info;

    private void Start()
    {
        info.SetActive(false);
    }

    private void Update()
    {
        if (GameManager._Stage == GameManager.Stage.Running)
        {
            #if UNITY_STANDALONE_WIN
                infoText.text = "Hold Space To Boost Speed!";
                info.SetActive(true);
            #endif

        }
        if (GameManager._Stage == GameManager.Stage.Painting)
        {
            #if UNITY_STANDALONE_WIN
                infoText.text = "Hold Space To Paint!";
                info.SetActive(true);
            #endif

        }
    }
}
