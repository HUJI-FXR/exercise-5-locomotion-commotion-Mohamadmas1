using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    [SerializeField] private GameObject playerPrefab;
    public GameObject player { get; private set; }

    [Header("Monster Spawning")]
    [SerializeField] private List<GameObject> monsterPrefabs;
    public int monsterCount;

    [Header("Score System")]
    [SerializeField] private float comboBonus;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    private int score;
    private int highScore;
    private float comboTimer;

    [Header("UI")]
    [SerializeField] private GameObject dieScreen;
    [SerializeField] private GameObject winScreen;

    void Start()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple GameManagers detected. Deleting this one.");
            Destroy(gameObject);
        }

        GetHighScore();
        Spawn();
        SpawnMonsters();
    }

    private void GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
            highScoreText.text = "High Score: " + highScore;
        }
        else
        {
            highScore = 0;
            highScoreText.text = "High Score: 0";
        }
    }

    private void Spawn()
    {
        dieScreen.SetActive(false);
        winScreen.SetActive(false);
        score = 0;
        scoreText.text = "Score: 0";
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        // disable cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            Vector3 position = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            Instantiate(monsterPrefab, position, Quaternion.identity);
        }
    }

    void Update()
    {
        comboTimer += Time.deltaTime;
        if (monsterCount == 0)
        {
            KillPlayer();
        }
    }

    public void AddScore()
    {
        score += (int)Mathf.Round(1f + comboBonus / comboTimer);
        comboTimer = 0;
        scoreText.text = "Score: " + score;
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "High Score: " + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void KillPlayer()
    {
        if (monsterCount == 0)
        {
            winScreen.SetActive(true);
        }
        else
        {
            dieScreen.SetActive(true);
        }
        Destroy(player);
        player = null;

        // enable cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
