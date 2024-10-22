using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject healthBar;
    [SerializeField] AudioClip gameOverSound;


    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject victoryScreen;

    private void Start()
    {
        if (creditsScreen != null)
            creditsScreen.SetActive(false);
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (pauseScreen != null)
            pauseScreen.SetActive(false);
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void CloseGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverScreen.activeInHierarchy && (victoryScreen == null || !victoryScreen.activeInHierarchy))
        {
            if (pauseScreen.activeInHierarchy)
            {
                //unpause game if already paused
                Pause(false);
            }
            else
            {
                //pause game
                Pause(true);
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Pause(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }  
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Credits()
    {
        startScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void ReturnMain() { 
        creditsScreen.SetActive(false);
        startScreen.SetActive(true);
    }
    public void Resume()
    {
        Pause(false);
    }

    public void SkipTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void VictoryScreen()
    {
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
    }

}