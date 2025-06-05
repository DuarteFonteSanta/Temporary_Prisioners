using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GameWin : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset winScene;

#endif

    [SerializeField] private string sceneToLoad;


    void OnValidate()
    {
#if UNITY_EDITOR
        if (winScene != null)
        {
            sceneToLoad = winScene.name;
        }

#endif
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Player2"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
