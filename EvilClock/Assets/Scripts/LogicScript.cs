using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{

    public GameObject gameWinScreen;
    public GameObject gameOverScreen;
    bool gameOn;

    public GameObject bar;
    public float maxTime = 10f;
    public float timeLeft;
    void Start()
    {
        timeLeft = maxTime;
        gameOn = true;
        Pillows.onPillowCollect += AddTime;
    }

    private void Update()
    {
        if (timeLeft > 0 && gameOn)
        {
            timeLeft -= Time.deltaTime;
            bar.transform.localScale = new Vector3(timeLeft / maxTime, 1, 1);
        }
        else if (timeLeft <= 0)
        {
            bar.transform.localScale = new Vector3(0, 1, 1);
            GameOver();

        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameWin()
    {
        gameWinScreen.SetActive(true);
        gameOn = false;
    }

    public void GameOver()
    {

        gameOverScreen.SetActive(true);
        gameOn = false;
    }

    public void AddTime(int amount)
    {
        if ((timeLeft + amount) >= maxTime)
        {
            timeLeft = maxTime;
        } 
        else 
        {
            timeLeft += amount;
        }
        
    }

    

}
