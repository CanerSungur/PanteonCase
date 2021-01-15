using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private Runner runner;
   
    private void Start()
    {
        animator = GetComponent<Animator>();
        runner = GetComponent<Runner>();
    }

    private void Update()
    {
        if (runner._State == Runner.State.Idle) animator.Play("Idle");
        else if (runner._State == Runner.State.Running) animator.Play("Running");
        else if (runner._State == Runner.State.Collided) animator.Play("Collided");
        else if (runner._State == Runner.State.Falling) animator.Play("Falling");
        else if (runner._State == Runner.State.Victory) animator.Play("Victory");
    }
}
