using UnityEngine;

public class Key : MonoBehaviour
{
    private bool _isPlayerNear = false;
    public Timer timer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = false;
        }
    }

    void Update()
    {
        if (_isPlayerNear && Input.GetKeyDown(KeyCode.M))
        {
            timer.KeyCaught();
        }
    }
}