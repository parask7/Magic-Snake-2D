using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private List<Transform> segments;

    public Transform segmentPrefab;

    private void Start()
    {
        segments = new List<Transform>();
        segments.Add(transform);
    }

    private void OnEnable()
    {
        SwipeInput.OnSwipe += HandleSwipe;
    }

    private void OnDisable()
    {
        SwipeInput.OnSwipe -= HandleSwipe;
    }

    private void Update()
    {
        // Do not take input if game is not running
        if (!GameManager.Instance.IsGameRunning())
            return;

        // Keyboard input (PC)
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
            direction = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
            direction = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
            direction = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
            direction = Vector2.right;
    }

    private void HandleSwipe(Vector2 swipeDir)
    {
        if (!GameManager.Instance.IsGameRunning())
            return;

        // Same direction rules as keyboard
        if (swipeDir == Vector2.up && direction != Vector2.down)
            direction = Vector2.up;
        else if (swipeDir == Vector2.down && direction != Vector2.up)
            direction = Vector2.down;
        else if (swipeDir == Vector2.left && direction != Vector2.right)
            direction = Vector2.left;
        else if (swipeDir == Vector2.right && direction != Vector2.left)
            direction = Vector2.right;
    }

    private void FixedUpdate()
    {
        // Stop movement when game is paused or over
        if (!GameManager.Instance.IsGameRunning())
            return;

        // Move body
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        // Move head on grid
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + direction.x,
            Mathf.Round(transform.position.y) + direction.y,
            0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);

        GameManager.Instance.AddScore(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.Instance.IsGameRunning())
            return;

        if (other.CompareTag("Food"))
        {
            Grow(); // food is NOT destroyed (your old logic)
        }
        else if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
