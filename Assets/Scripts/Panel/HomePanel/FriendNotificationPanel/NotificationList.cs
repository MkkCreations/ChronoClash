using System;
using UnityEngine;
using UnityEngine.Networking;

public class NotificationList : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject friendNotifTemplate;

    private void OnEnable()
    {
        var request = UnityWebRequest.Get(HttpConst.FRIEND_NOTIFICATIONS.Value);
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.GetFriendNotifs(request, ShowNotifications));
    }

    public void ShowNotifications()
    {
        ResetList();
        foreach (FriendNotifications.FriendNotif notif in FriendNotifications.instance.list.notifications)
        {
            GameObject notifInst = Instantiate(friendNotifTemplate);
            notifInst.transform.SetParent(scrollView.transform);
            notifInst.GetComponent<FriendNotifTemplate>().SetData(notif.id, notif.username, notif.image, notif.level.level, DateTime.Parse(notif.date), ShowNotifications);
        }
    }

    private void ResetList()
    {
        foreach (FriendNotifTemplate person in scrollView.transform.GetComponentsInChildren<FriendNotifTemplate>())
            Destroy(person.gameObject);
    }
}
