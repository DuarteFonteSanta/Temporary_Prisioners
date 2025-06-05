using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class GameManager : MonoBehaviour
{

#if UNITY_EDITOR
    [SerializeField] private SceneAsset gameWinScene;
    [SerializeField] private SceneAsset gameOverScene;

#endif

    [SerializeField] private string gameWinToLoad;

    [SerializeField] private string gameOverToLoad;


    void OnValidate()
    {
#if UNITY_EDITOR
        if (gameWinScene != null)
        {
            gameWinToLoad = gameWinScene.name;
        }

        if (gameOverScene != null)
        {
            gameOverToLoad = gameOverScene.name;
        }
#endif
    }

    public GameObject gameOverUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject gameWinUI;


    public void GameOver()
    {
        SceneManager.LoadScene(gameOverToLoad);
    }

    public void GameWin()
    {
        SceneManager.LoadScene(gameWinToLoad);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinGameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
