using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject healthBar;
    [SerializeField] AudioClip gameOverSound;


    [SerializeField] GameObject pauseScreen;

    private void Start()
    {
        healthBar.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        healthBar.SetActive(false);
        gameOverScreen.SetActive(true);
        SoundManager.Instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Credits()
    {
        creditsScreen.SetActive(true);
    }

}