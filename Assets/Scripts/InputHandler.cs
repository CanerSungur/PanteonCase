using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    /*
     * 
     * Detects touch input from user.
     * Decides speed multiplier according to touch volume.
     * Sends speed info to the objects.
     * 
     */

    public event EventHandler OnTouchEnded;

    private Action HandleRunningPhaseTouch;
    private Action HandlePaintPhaseTouch;

    private Touch touch;
    private Vector2 firstTouch;
    private Vector2 lastTouch;
    private Vector3 touchDeltaPosition;

    private float horizontalSpeedModifier;
    private float verticalSpeedModifier;
    private string horizontalDirection;
    private string verticalDirection;

    private Runner player;
    private PlayerController playerController;

    // These are public because they are disabled at the start of the game.
    public Spray spray;
    public SprayController sprayController;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Runner>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        HandleRunningPhaseTouch += UpdateTouchDirection;
        HandleRunningPhaseTouch += UpdateHorizontalSpeedModifier;
        HandleRunningPhaseTouch += UpdateVerticalSpeedModifierForRunning;

        HandlePaintPhaseTouch += UpdateTouchDirection;
        HandlePaintPhaseTouch += UpdateHorizontalSpeedModifier;
        HandlePaintPhaseTouch += UpdateVerticalSpeedModifierForPainting;
    }

    private void Update()
    {

        #region Handle Touch

        if (GameManager._Stage == GameManager.Stage.Running)
            HandleRunningPhaseTouch();
        else if (GameManager._Stage == GameManager.Stage.Painting)
            HandlePaintPhaseTouch();
        else
            Debug.Log("Something went wrong while handling touch.");

        #endregion


        if ((playerController.isEnabled && !sprayController.enabled) || (!playerController.isEnabled && sprayController.enabled))
        {// Because even one of the script is disabled, this script makes both active. That's why we put this condition.

            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    player._State = Runner.State.Running;
                    spray._State = Spray.State.Painting;

                    firstTouch = touch.position;

                    // To avoid going left on first touch.
                    lastTouch = firstTouch;

                    // We set rotation 0 to avoid player's animation rotation change.
                    player.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    lastTouch = touch.position;

                    touchDeltaPosition = touch.deltaPosition; // For spray control.
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    player._State = Runner.State.Idle;
                    spray._State = Spray.State.Idle;

                    // Reset values when touch ends.
                    firstTouch = new Vector2(0, 0);
                    lastTouch = new Vector2(0, 0);
                    horizontalSpeedModifier = 0f;
                    horizontalDirection = "None";
                    verticalSpeedModifier = 0f;
                    verticalDirection = "None";
                }
            }
        }
    }

    private void UpdateTouchDirection()
    {
        #region Vertical

        if (firstTouch.y > lastTouch.y)
            verticalDirection = "Down";
        else if (firstTouch.y < lastTouch.y)
            verticalDirection = "Up";
        else if (firstTouch.y == lastTouch.y)
            verticalDirection = "None";

        #endregion

        #region Horizontal

        if (firstTouch.x < lastTouch.x)
        {
            if (lastTouch.x - firstTouch.x <= 15)
                horizontalDirection = "None";
            else
                horizontalDirection = "Right";
        }
        else if (firstTouch.x > lastTouch.x)
        {
            if (firstTouch.x - lastTouch.x <= 15)
                horizontalDirection = "None";
            else
                horizontalDirection = "Left";
        }
        else if (firstTouch.x == lastTouch.x)
            horizontalDirection = "None";

        #endregion
    }

    private void UpdateVerticalSpeedModifierForRunning()
    {
        float distance = GetTouchDistance();

        if (verticalDirection == "Up") // This way, in running phase we cannot move back.
        {
            if (distance >= 0f && distance < 10f)
                verticalSpeedModifier = .06f;
            else if (distance >= 10f && distance < 20f)
                verticalSpeedModifier = .08f;
            else if (distance >= 20f && distance < 30f)
                verticalSpeedModifier = .1f;
            else if (distance >= 30f && distance < 40f)
                verticalSpeedModifier = .2f;
            else if (distance >= 40f)
                verticalSpeedModifier = .3f;
            else
                Debug.Log("Something went wrong with Horizontal Speed Modifier!");
        }
    }

    private void UpdateVerticalSpeedModifierForPainting()
    {
        float distance = GetTouchDistance();

        // This way, in painting phase we can move to every direction.
        if (distance >= 0f && distance < 20f)
            verticalSpeedModifier = 0f;
        else if (distance >= 20f && distance < 40f)
            verticalSpeedModifier = .02f;
        else if (distance >= 40f && distance < 60f)
            verticalSpeedModifier = .03f;
        else if (distance >= 60f && distance < 80f)
            verticalSpeedModifier = .04f;
        else if (distance >= 80f)
            verticalSpeedModifier = .05f;
        else
            Debug.Log("Something went wrong with Horizontal Speed Modifier!");
    }

    private void UpdateHorizontalSpeedModifier()
    {
        float distance = GetTouchDistance();

        if (distance >= 0f && distance < 30f)
            horizontalSpeedModifier = .05f;
        else if (distance >= 30f && distance < 40f)
            horizontalSpeedModifier = .05f;
        else if (distance >= 40f && distance < 50f)
            horizontalSpeedModifier = .1f;
        else if (distance >= 50f)
            horizontalSpeedModifier = .12f;
        else
            Debug.Log("Something went wrong with Vertical Speed Modifier!");
    }

    public float GetHorizontalSpeedModifier()
    {
        return horizontalSpeedModifier;
    }

    public float GetVerticalSpeedModifier()
    {
        return verticalSpeedModifier;
    }

    public string GetHorizontalDirection()
    {
        return horizontalDirection;
    }

    public string GetVerticalDirection()
    {
        return verticalDirection;
    }

    public Vector2 GetTouchDeltaPosition()
    {
        return touchDeltaPosition;
    }

    private float GetTouchDistance()
    {
        return Vector2.Distance(firstTouch, lastTouch);
    }
}
