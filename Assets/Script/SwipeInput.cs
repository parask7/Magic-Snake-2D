using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    public float minSwipeDistance = 50f;

    private Vector2 startPos;
    private Vector2 endPos;

    public static System.Action<Vector2> OnSwipe;

    void Update()
    {
        // TOUCH INPUT
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startPos = touch.position;

            if (touch.phase == TouchPhase.Ended)
                DetectSwipe(touch.position);
        }

        // MOUSE INPUT (Editor + WebGL)
        if (Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
            DetectSwipe(Input.mousePosition);
    }

    void DetectSwipe(Vector2 endPosition)
    {
        endPos = endPosition;
        Vector2 swipe = endPos - startPos;

        if (swipe.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            OnSwipe?.Invoke(swipe.x > 0 ? Vector2.right : Vector2.left);
        }
        else
        {
            OnSwipe?.Invoke(swipe.y > 0 ? Vector2.up : Vector2.down);
        }
    }
}
