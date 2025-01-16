using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public string horizontalInputAxis = "Horizontal"; // Customizable input axis for movement

    [Header("Laser Settings")]
    public Projectile laserPrefab; // Laser prefab to instantiate
    private Projectile laser; // Reference to the active laser

    [Header("Audio Settings")]
    public AudioClip laserShotSound; // Laser shot sound effect
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to the player
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

        // Get horizontal input and move the player
        float moveInput = Input.GetAxis(horizontalInputAxis);
        position.x += moveInput * speed * Time.deltaTime;

        // Clamp the position to prevent the player from moving out of bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        transform.position = position;
    }

    private void HandleShooting()
    {
        // Check if Space is pressed and there's no active laser
        if (laser == null && Input.GetKeyDown(KeyCode.Space))
        {
            // Instantiate the laser and set its direction
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.up = Vector3.up;

            // Play laser shot sound
            if (audioSource && laserShotSound)
            {
                audioSource.PlayOneShot(laserShotSound);
            }

            // Reset laser reference when it's destroyed
            StartCoroutine(ResetLaserWhenDestroyed(laser));
        }
    }

    private System.Collections.IEnumerator ResetLaserWhenDestroyed(Projectile laserInstance)
    {
        // Wait until the laser is destroyed, then reset the reference
        while (laserInstance != null)
        {
            yield return null;
        }
        laser = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with missiles or invaders
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            // Notify the GameManager that the player has been killed
            GameManager.Instance?.OnPlayerKilled(this);
        }
    }
}
