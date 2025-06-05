using TMPro;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textField;

    [SerializeField] private TextMeshProUGUI toolTip;

    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(playerController.GetControl("Close")))
        {
            ShowPanel(false);
        }
    }

    public void ShowPanel(bool b)
    {
        panel.SetActive(b);
    }

    public void DisplayMessage(string message)
    {
        textField.text = message;
        ShowPanel(true);
    }

    public void ShowToolTip(bool b)
    {
        toolTip.gameObject.SetActive(b);
    }

    public void DisplayToolTip(string message)
    {
        ShowToolTip(true);
        toolTip.text = message;
    }

    public void SetOwner(PlayerController controller) => playerController = controller;
}
