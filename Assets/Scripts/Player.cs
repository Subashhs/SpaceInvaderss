using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))] // Ensures the AudioSource component is on the player GameObject
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    private Projectile laser;

    public AudioClip laserShotSound; // The sound to play when the laser is fired
    public AudioClip playerDeathSound; // Sound to play when the player is killed
    public float laserCooldown = 0.5f; // Time between laser shots
    private float lastLaserTime = -1f; // Track the time of the last laser shot
    private AudioSource audioSource;  // The AudioSource component to play sounds

    private Rigidbody2D rb;

    public bool isPlayer1 = false; // Flag to distinguish between Player1 and Player2

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the player
    }

    private void Update()
    {
        if (isPlayer1)
        {
            HandlePlayer1Movement(); // Handle movement and laser fire for Player 1
        }
        else
        {
            HandlePlayer2Movement(); // Handle movement and laser fire for Player 2
        }
        HandleLaserFire(); // Handle laser firing for both players
    }

    // Player 1 Movement: A for left, D for right
    private void HandlePlayer1Movement()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f; // Move left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f; // Move right
        }

        // Apply movement (scale by speed and deltaTime)
        Vector2 currentPosition = rb.position;
        float moveX = horizontalInput * speed * Time.deltaTime;

        // Clamp the position to screen bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        currentPosition.x = Mathf.Clamp(currentPosition.x + moveX, leftEdge.x, rightEdge.x);

        // Move using Rigidbody2D's MovePosition for better physics interaction
        rb.MovePosition(currentPosition);
    }

    // Player 2 Movement: Arrow keys (or can be customized further)
    private void HandlePlayer2Movement()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f; // Move left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f; // Move right
        }

        // Apply movement (scale by speed and deltaTime)
        Vector2 currentPosition = rb.position;
        float moveX = horizontalInput * speed * Time.deltaTime;

        // Clamp the position to screen bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        currentPosition.x = Mathf.Clamp(currentPosition.x + moveX, leftEdge.x, rightEdge.x);

        // Move using Rigidbody2D's MovePosition for better physics interaction
        rb.MovePosition(currentPosition);
    }

    // Handle laser firing for both players
    private void HandleLaserFire()
    {
        // Ensure there's enough time between laser shots (cooldown)
        if (laser == null && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (Time.time - lastLaserTime >= laserCooldown)
            {
                // Create laser slightly in front of the player (e.g., adjusting based on sprite size or desired offset)
                Vector3 laserSpawnPosition = transform.position + Vector3.up * 0.5f; // Adjust offset if needed
                laser = Instantiate(laserPrefab, laserSpawnPosition, Quaternion.identity);

                // Play laser shot sound
                if (laserShotSound != null)
                {
                    audioSource.PlayOneShot(laserShotSound); // Play the sound when the laser is fired
                }

                // Update last laser shot time
                lastLaserTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with invader or missile (player death)
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            // Play player death sound
            if (playerDeathSound != null)
            {
                audioSource.PlayOneShot(playerDeathSound);
            }

            // Call GameManager to handle player death
            GameManager.Instance.OnPlayerKilled(this);
            gameObject.SetActive(false);  // Disable the player object
        }
    }
}
