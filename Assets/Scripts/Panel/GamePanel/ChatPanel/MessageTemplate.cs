using UnityEngine;
using TMPro;

public class MessageTemplate : MonoBehaviour
{
    public TMP_Text user;
    public TMP_Text message;

    public void SetData(string user, string message)
    {
        this.user.text = user;
        this.message.text = message;
    }

    public void OnAcceptFriend()
    {
        
    }
}
