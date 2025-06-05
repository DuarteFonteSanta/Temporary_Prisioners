using UnityEngine;

[CreateAssetMenu(menuName = "Tooltip")]
public class Tooltip : ScriptableObject
{
    public Texture2D logo;
    [TextArea(10, 20)] public string tooltipMessage;

    public string GetToolTipText(KeyCode code) => tooltipMessage.Replace("{KeyCode}", code.ToString().ToUpper());
}
