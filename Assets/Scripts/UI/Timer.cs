using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private float timeDuration = 5f * 60f;
    private float timer;

    [SerializeField]
    private TextMeshProUGUI timerUI;
    
    private bool wasCaught = false;
    private float timeSinceCaught = 0f;
    private float fadeDuration = 2f;
    
    public GameManager gameManager;

    
    void Start()
    {
        timer = timeDuration;
        UpdateMesh();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateMesh();
        }
        else
        {
            timer = 0;
            UpdateMesh();
            gameManager.GameOver();
        }
        if (wasCaught)
        {
            if (timeSinceCaught < fadeDuration)
            {
                timeSinceCaught += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(timeSinceCaught / fadeDuration);
                timerUI.color = Color.Lerp(Color.red, Color.white, normalizedTime);
            }
            else
            {
                wasCaught = false;
                timerUI.color = Color.white;
                timeSinceCaught = 0;
            }
            
            
        }
        
        
    }

    public void PlayerCaught()
    {
        Debug.Log("Player Caught");
        timer -= 30f;
        gameManager.GameOver();
        if (timer <= 0)
        {
            timer = 0;
            UpdateMesh();
            gameManager.GameOver();
        }
        UpdateMesh();
        timerUI.color = Color.red;
        wasCaught = true;
        timeSinceCaught = 0f;
    }

    public void KeyCaught()
    {
        Debug.Log("Key Caught");
        timer = 0;
        timerUI.color = Color.green;
        gameManager.GameWin();
        UpdateMesh();
    }
    
    private void UpdateMesh()
    {
        float minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = Mathf.FloorToInt(timer % 60f);
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
