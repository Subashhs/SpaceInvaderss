using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Laser Settings")]
    public Projectile laserPrefab;
    private Projectile laser;

    [Header("Audio Settings")]
    public AudioClip laserShotSound;  // Audio clip for laser shot sound
    public AudioClip missileHitSound; // Audio clip for missile hit sound
    private AudioSource audioSource;  // Reference to the AudioSource

    private void Start()
    {
        // Get the AudioSource component on the player
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        Vector3 position = transform.position;

        // Use arrow keys for movement (for Player2)
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.x += speed * Time.deltaTime;
        }

        // Clamp position to the screen bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        transform.position = position;
    }

    private void HandleShooting()
    {
        // Allow shooting with the "W" key
        // Allow shooting with the "W" key
        if (laser == null && Input.GetKeyDown(KeyCode.W))
        {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.up = Vector3.up;

            // Play the laser shot sound when firing
            if (audioSource && laserShotSound)
            {
                audioSource.PlayOneShot(laserShotSound);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player is hit by a missile or invader
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            // Call GameManager method when the player is killed
            GameManager.Instance.OnPlayerKilled(this);

            // Play the missile hit sound when the player is hit
            if (audioSource && missileHitSound)
            {
                audioSource.PlayOneShot(missileHitSound);
            }
        }
    }
}
