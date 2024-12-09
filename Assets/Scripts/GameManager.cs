using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int waveNumber = 0;
    public GameObject myPrefab;
    public static int score = 0;
    public static int livesLeft = 10;
    public int highScore = 0;

    void Start()
    {
        if (highScore == 0)
        {
            highScore = PlayerPrefs.GetInt("highScore", 0);
            Debug.Log("High Score is: " + highScore);
        }
        SpawnMoreAndMore();
    }

    public void SpawnOne()
    {

        GameObject newlyCreated =
                    Object.Instantiate(myPrefab);
        newlyCreated.transform.position =
                        new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(7, 17));
        newlyCreated.transform.LookAt(Camera.main.transform);
    }


    public void SpawnMoreAndMore()
    {
        if (waveNumber > 10)
        {
            return;
        }

        waveNumber += 2;
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnOne();
        }
        Invoke("SpawnMoreAndMore", 3);
    }


    public void EndLife()
    {
        if (livesLeft <= 0)
        {
            Debug.Log("Game Over! You scored " + score);
            if (score > highScore)
            {
                Debug.Log("New High Score!!!");
                highScore = score;
                PlayerPrefs.SetInt("highScore", highScore);
            }
            livesLeft = 100;
            score = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        else
        {
            livesLeft--;
            Debug.Log("Oops! You have " + livesLeft + " lives left");
        }
    }
}