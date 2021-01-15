using UnityEngine;

public class Spray : MonoBehaviour
{
    public enum State
    {
        Idle,
        Painting
    }

    [HideInInspector]public State _State;

    private void Start()
    {
        _State = State.Idle;
    }
}
