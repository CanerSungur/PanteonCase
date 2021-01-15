using UnityEngine;

public class FinishLineCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager._Stage = GameManager.Stage.Painting;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Opponent"))
        {
            other.gameObject.GetComponent<Runner>()._State = Runner.State.Victory;
        }
    }
}
