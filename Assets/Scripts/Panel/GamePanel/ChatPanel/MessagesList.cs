using UnityEngine;
using UnityEngine.UI;

public class MessagesList : MonoBehaviour
{
    public static MessagesList instance;
    public GameObject scrollView;
    public GameObject messageTamplate;

    private void Awake()
    {
        instance = this;
    }

    public void AddMessage(Message msg)
    {
        GameObject messageInst = Instantiate(messageTamplate);
        if (User.instance.user.user.username == msg.user)
            messageInst.GetComponent<Image>().color = new Color(0.832f, 0, 1, 0.25f);
        else
            messageInst.GetComponent<Image>().color = new Color(0.2f, 0, 0.3f, 0.25f);
        messageInst.transform.SetParent(scrollView.transform);
        messageInst.GetComponent<MessageTemplate>().SetData(msg.user, msg.message);
    }

    private void ResetList()
    {
        foreach (MessageTemplate message in scrollView.transform.GetComponentsInChildren<MessageTemplate>())
            Destroy(message.gameObject);
    }
}
