using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GestureDirection
{
    None, Left, Right, Up, Down
}
public class Gesture : MonoBehaviour
{
    public static GestureDirection Direction
    {
        get
        {
            if (_singleton == null)
            {
                Debug.LogError("Gesture Controller is not instance!");
                return GestureDirection.None;
            }
            return _singleton.direction;
        }
    }
    public static bool Enable { get; set; }
    private static Gesture _singleton;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool swipeDetected = false;
    private GestureDirection direction;
    bool disableSwipe;

    private void Awake()
    {
        if (_singleton != null)
        {
            Destroy(_singleton.gameObject);
        }
        _singleton = this;
        Enable = true;
    }

    private void Update()
    {
        if (!Enable)
            return;
        DetectSwipe();
        if(direction != GestureDirection.None && !disableSwipe)
        {
            disableSwipe = true;
            return;
        }
        if (disableSwipe)
        {
            direction = GestureDirection.None;
            disableSwipe = false;
        }
    }

    private void DetectSwipe()
    {
        // Detect touch or mouse input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    swipeDetected = false;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    swipeDetected = true;
                    break;
            }
        }
        else if (Input.GetMouseButtonDown(0)) // For mouse input (useful for testing in the editor)
        {
            startTouchPosition = Input.mousePosition;
            swipeDetected = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            swipeDetected = true;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            direction = GestureDirection.Up;
        }else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = GestureDirection.Down;
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = GestureDirection.Left;
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = GestureDirection.Right;
        }

        if (swipeDetected)
        {
            DetectSwipeDirection();
        }
    }

    private void DetectSwipeDirection()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            // Horizontal swipe
            if (swipeDirection.x > 0)
                direction = GestureDirection.Right;
            else
                direction = GestureDirection.Left;
        }
        else
        {
            // Vertical swipe
            if (swipeDirection.y > 0)
                direction = GestureDirection.Up;
            else
                direction = GestureDirection.Down;
        }

        swipeDetected = false;
    }
}
