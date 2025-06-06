using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GameWin : TooltipObject
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset winScene;
    [SerializeField] private SceneAsset winScene2;

#endif

    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneToLoad2;

    private int playersInZone = 0;


    void OnValidate()
    {
#if UNITY_EDITOR
        if (winScene != null)
        {
            sceneToLoad = winScene.name;
            sceneToLoad2 = winScene2.name;
        }

#endif
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Player2"))
        {
            playersInZone++;
            Debug.Log("Player entered. Total players in zone: " + playersInZone);
        }

        if (playersInZone >= 2)
        {
            Debug.Log("At least two players are in the zone!");
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Player2"))
        {
            playersInZone = Mathf.Max(0, playersInZone - 1); // avoid negative count
            Debug.Log("Player left. Total players in zone: " + playersInZone);
        }
    }

    public void Win2()
    {
        SceneManager.LoadScene(sceneToLoad2);
    }

}
