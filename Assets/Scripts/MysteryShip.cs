using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MysteryShip : MonoBehaviour
{
    public float speed = 5f;         // Speed at which the MysteryShip moves
    public float cycleTime = 30f;    // Time it takes to complete a cycle (left to right or right to left)
    public int score = 100;          // Score awarded when the mystery ship is destroyed
    public bool isPlayer1Target = true;  // Determines which player gets the score when the ship is killed

    private Vector2 leftDestination;  // Left edge destination point
    private Vector2 rightDestination; // Right edge destination point
    private int direction = -1;       // Direction (-1 for left, 1 for right)
    private bool spawned = false;     // Whether the ship has spawned

    private void Start()
    {
        // Transform the viewport to world coordinates so we can set the mystery
        // ship's destination points
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Offset each destination by 1 unit so the ship is fully out of sight
        leftDestination = new Vector2(leftEdge.x - 1f, transform.position.y);
        rightDestination = new Vector2(rightEdge.x + 1f, transform.position.y);

        // Start the ship cycle
        Despawn();
    }

    private void Update()
    {
        if (!spawned) return;

        // Move the ship depending on the direction
        if (direction == 1)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        // Move the ship to the right
        transform.position += speed * Time.deltaTime * Vector3.right;

        // If the ship reaches the right edge, despawn
        if (transform.position.x >= rightDestination.x)
        {
            Despawn();
        }
    }

    private void MoveLeft()
    {
        // Move the ship to the left
        transform.position += speed * Time.deltaTime * Vector3.left;

        // If the ship reaches the left edge, despawn
        if (transform.position.x <= leftDestination.x)
        {
            Despawn();
        }
    }

    private void Spawn()
    {
        // Flip the direction when the ship is respawned
        direction *= -1;

        // Set the position of the ship based on its direction
        if (direction == 1)
        {
            transform.position = leftDestination;
        }
        else
        {
            transform.position = rightDestination;
        }

        spawned = true;
    }

    private void OnDestroy()
    {
        // Notify the GameManager when the mystery ship is destroyed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMysteryShipKilled(this);  // Pass this object to GameManager
        }
    }

    private void Despawn()
    {
        // Stop the ship from moving
        spawned = false;

        // Set the ship position offscreen based on its current direction
        if (direction == 1)
        {
            transform.position = rightDestination;
        }
        else
        {
            transform.position = leftDestination;
        }

        // Respawn the ship after the cycle time
        Invoke(nameof(Spawn), cycleTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the ship was hit by a laser
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn(); // Stop the ship from moving
            GameManager.Instance.OnMysteryShipKilled(this);  // Notify the GameManager
        }
    }
}
