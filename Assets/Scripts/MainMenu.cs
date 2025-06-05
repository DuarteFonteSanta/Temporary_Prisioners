using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset gameScene;
    [SerializeField] private SceneAsset instructionsMenu;
    [SerializeField] private SceneAsset mainMenu;
#endif

    [SerializeField] private string sceneToLoad;

    [SerializeField] private string instructionsScene;

    [SerializeField] private string mainMenuScene;

    void OnValidate()
    {
#if UNITY_EDITOR
        if (gameScene != null)
        {
            sceneToLoad = gameScene.name;
        }

        if (instructionsMenu != null)
        {
            instructionsScene = instructionsMenu.name;
        }

        if (mainMenu != null)
        {
            mainMenuScene = mainMenu.name;
        }
#endif
    }

    public void Play()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Instructions()
    {
        SceneManager.LoadScene(instructionsScene);
    }

    public void closeInstructions()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}