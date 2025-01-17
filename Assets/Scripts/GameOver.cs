using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button retryButton;

    private void Start()
    {
        gameOverPanel.SetActive(false);

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RetryGame);
        }
        else
        {
            Debug.LogError("Retry Button is not assigned in the GameOverManager.");
        }
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void RetryGame()
    {
        gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
        ResetGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetGame()
    {
        GameManager.Instance.ResetGame();

    }
}
