using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreText2;
    [SerializeField] private Text livesText;
    [SerializeField] private Text livesText2;

    [Header("Game Elements")]
    private Player player;
    private Player2 player2; // Assuming Player2 script exists
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    public int score { get; private set; } = 0;
    public int score2 { get; private set; } = 0;
    public int lives { get; private set; } = 3;
    public int lives2 { get; private set; } = 3;

    private void Awake()
    {
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
        player = FindObjectOfType<Player>();
        player2 = FindObjectOfType<Player2>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

        // Start a new game
        NewGame();

        // If both players start with 0 lives, show the game over UI initially.
        if (lives <= 0 && lives2 <= 0)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            gameOverUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Only check for restarting if the game is over
        if (lives <= 0 && lives2 <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                NewGame();
            }
        }
    }

    private void NewGame()
    {
        gameOverUI.SetActive(false);

        SetScore(0);
        SetScore2(0);
        SetLives(3);
        SetLives2(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        foreach (var bunker in bunkers)
        {
            bunker.ResetBunker();
        }

        RespawnPlayers();
    }

    private void RespawnPlayers()
    {
        RespawnPlayer(player);
        RespawnPlayer(player2);
    }

    private void RespawnPlayer(MonoBehaviour player)
    {
        if (player == null) return;

        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
    }

    private void SetScore(int newScore)
    {
        score = newScore;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    private void SetScore2(int newScore2)
    {
        score2 = newScore2;
        scoreText2.text = score2.ToString().PadLeft(4, '0');
    }

    private void SetLives(int newLives)
    {
        lives = Mathf.Max(newLives, 0);
        livesText.text = lives.ToString();
    }

    private void SetLives2(int newLives2)
    {
        lives2 = Mathf.Max(newLives2, 0);
        livesText2.text = lives2.ToString();
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);

        player.gameObject.SetActive(false);

        if (lives > 0 || lives2 > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    public void OnPlayerKilled(Player2 player2)
    {
        SetLives2(lives2 - 1);

        player2.gameObject.SetActive(false);

        if (lives > 0 || lives2 > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        SetScore(score + invader.score);

        if (invaders.GetAliveCount() == 0)
        {
            NewRound();
        }
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
        SetScore2(score2 + mysteryShip.score);
    }

    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);

            if (player.gameObject.activeSelf)
            {
                OnPlayerKilled(player);
            }
            if (player2.gameObject.activeSelf)
            {
                OnPlayerKilled(player2);
            }
        }
    }
}
