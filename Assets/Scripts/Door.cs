using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private bool isOpen = false;
    private Coroutine closeCoroutine;
    private bool _isPlayer1Near = false;
    private bool _isPlayer2Near = false;

    [SerializeField] private float closeDelay = 1f; // Delay after opening

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen && (other.CompareTag("Player") || other.CompareTag("Player2")))
        {
            Open();
            OnDoorOpened();
        }
        if (other.CompareTag("Player2"))
        {
            _isPlayer2Near = true;
        }
        if (other.CompareTag("Player"))
        {
            _isPlayer1Near = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player2"))
        {
            _isPlayer2Near = false;
        }
        if (other.CompareTag("Player"))
        {
            _isPlayer1Near = false;
        }
    }


    public void Open()
    {
        isOpen = true;
        _animator.SetTrigger("Open");

        // Cancel any existing close coroutine to prevent stacking
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }

        closeCoroutine = StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);
        Close();
        OnDoorClosed();
    }

    private void Close()
    {
        isOpen = false;
        _animator.SetTrigger("Close");
    }

    // Optional: walkability update hooks
    public void OnDoorOpened()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void OnDoorClosed()
    {
        gameObject.layer = LayerMask.NameToLayer("Unwalkable");
    }

    void Update()
    {
        if (!isOpen && (_isPlayer2Near && Input.GetKeyDown(KeyCode.E)))
        {
            Open();
            OnDoorOpened();
        }
        if (!isOpen && (_isPlayer1Near && Input.GetKeyDown(KeyCode.M)))
        {
            Open();
            OnDoorOpened();
        }
    }
}
