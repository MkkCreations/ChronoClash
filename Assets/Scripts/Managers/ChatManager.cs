using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{
    public static ChatManager instance;
    public TMP_InputField messageInput;
    public List<Message> messages;
    public Button showChatButton;
    public GameObject chatPanel;

    private void Awake()
    {
        instance = this;
        messages = new();
    }

    public void OnShowChat()
    {
        chatPanel.SetActive(true);
        showChatButton.gameObject.SetActive(false);
    }

    public void OnCloseChat()
    {
        chatPanel.SetActive(false);
        showChatButton.gameObject.SetActive(true);
    }

    public void SendMessage()
    {
        if (messageInput.text != "")
        {
            photonView.RPC("PushMessage", RpcTarget.All, User.instance.user.user.username, messageInput.text);
        }
    }

    [PunRPC]
    void PushMessage(string user, string msg)
    {
        var message = new Message()
        {
            user = user,
            message = msg
        };
        messages.Add(message);
        MessagesList.instance.AddMessage(message);
        messageInput.text = "";
    }
}
