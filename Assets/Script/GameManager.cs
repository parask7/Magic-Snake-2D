using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;   // NEW

    [Header("Start Panel")]
    public GameObject startPanel;
    public Button startButton;
    public Button quitButton;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button exitButton;

    private int score;
    private int highScore;                  // NEW
    private bool isGameRunning;

    private const string HIGH_SCORE_KEY = "HighScore"; // NEW

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Initial state
        Time.timeScale = 0f;
        isGameRunning = false;

        score = 0;

        // LOAD HIGH SCORE FROM LOCAL STORAGE
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        UpdateScoreUI();
        UpdateHighScoreUI();

        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);

        // Button listeners
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
        exitButton.onClick.AddListener(QuitGame);
    }

    // =========================
    // GAME FLOW
    // =========================

    public void StartGame()
    {
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
        isGameRunning = true;
    }

    public void GameOver()
    {
        isGameRunning = false;
        Time.timeScale = 0f;

        // SAVE high score only once
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save();

        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // =========================
    // SCORE
    // =========================

    public void AddScore(int amount)
    {
        if (!isGameRunning) return;

        score += amount;
        UpdateScoreUI();

        if (score > highScore)
        {
            highScore = score;
            UpdateHighScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    // =========================
    // HIGH SCORE
    // =========================


    private void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    // =========================
    // GAME STATE CHECK
    // =========================

    public bool IsGameRunning()
    {
        return isGameRunning;
    }
}
