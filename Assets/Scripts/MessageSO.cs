using UnityEngine;

[CreateAssetMenu(menuName = "Message")]
public class MessageSO : ScriptableObject
{
    [TextArea(10, 100)]
    public string message;

}
