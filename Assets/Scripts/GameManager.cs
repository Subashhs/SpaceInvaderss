using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // UI References
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText1;
    [SerializeField] private Text scoreText2;
    [SerializeField] private Text livesText1;
    [SerializeField] private Text livesText2;

    // Player and game-related references
    private Player player1;
    private Player player2;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;
    public AudioClip backgroundMusic;  // The background music to play
    private AudioSource audioSource;

    // Player scores and lives
    public int score1 { get; private set; } = 0;
    public int score2 { get; private set; } = 0;
    public int lives1 { get; private set; } = 3;
    public int lives2 { get; private set; } = 3;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensures only one GameManager instance exists
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Start playing the background music and loop it
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;  // Make sure the music loops
            audioSource.Play(); // Start playing the background music
        }

        // Find the players in the scene
        player1 = FindObjectsOfType<Player>()[0]; // Assuming Player 1 is the first in the scene
        player2 = FindObjectsOfType<Player>()[1]; // Assuming Player 2 is the second in the scene
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

        NewGame();
    }

    private void Update()
    {
        // Restart the game if no lives are left and Return key is pressed
        if ((lives1 <= 0 && lives2 <= 0) && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        gameOverUI.SetActive(false);

        // Reset the UI for Player 1 and Player 2
        SetScore1(0);
        SetScore2(0);
        SetLives1(3);
        SetLives2(3);

        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        // Custom respawn position for players
        player1.transform.position = new Vector3(-3f, -4f, 0f); // Position Player 1 on the left
        player2.transform.position = new Vector3(3f, -4f, 0f);  // Position Player 2 on the right
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
        audioSource.Stop(); // Stop background music
    }

    private void SetScore1(int score)
    {
        score1 = score;
        scoreText1.text = score1.ToString().PadLeft(4, '0');
    }

    private void SetScore2(int score)
    {
        score2 = score;
        scoreText2.text = score2.ToString().PadLeft(4, '0');
    }

    private void SetLives1(int lives)
    {
        lives1 = Mathf.Max(lives, 0);
        livesText1.text = lives1.ToString();
    }

    private void SetLives2(int lives)
    {
        lives2 = Mathf.Max(lives, 0);
        livesText2.text = lives2.ToString();
    }

    public void OnPlayer1Killed(Player player)
    {
        SetLives1(lives1 - 1);
        player1.gameObject.SetActive(false);

        if (lives1 > 0)
        {
            Invoke(nameof(NewRound), 1f); // Brief delay before new round starts
        }
        else
        {
            GameOver();
        }
    }

    public void OnPlayer2Killed(Player player)
    {
        SetLives2(lives2 - 1);
        player2.gameObject.SetActive(false);

        if (lives2 > 0)
        {
            Invoke(nameof(NewRound), 1f); // Brief delay before new round starts
        }
        else
        {
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        // Assume that invader score is assigned correctly in Invader class
        if (invader.isPlayer1Target)
        {
            SetScore1(score1 + invader.score); // Player 1 kills invader
        }
        else
        {
            SetScore2(score2 + invader.score); // Player 2 kills invader
        }

        if (invaders.GetAliveCount() == 0)
        {
            NewRound();
        }
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        if (mysteryShip.isPlayer1Target)
        {
            SetScore1(score1 + mysteryShip.score); // Player 1 gets the score
        }
        else
        {
            SetScore2(score2 + mysteryShip.score); // Player 2 gets the score
        }
    }

    public void OnBoundaryReached()
    {
        // Handle boundary reached by invaders
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayer1Killed(player1); // Handle Player 1's death when boundary reached
        }
    }
}
