using System;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FriendNotifTemplate : MonoBehaviour
{
    private string id;
    public RawImage avatar;
    public TMP_Text username;
    public TMP_Text level;
    public TMP_Text date;
    private Action showNotifications;

    public void SetData(string id, string username, string avatar, int level, DateTime date, Action showNotifications)
    {
        this.id = id;
        if (avatar.Length != 0)
        {
            this.avatar.texture = ImageTools.CreateTextureFromString(avatar);
        }
        this.username.text = username;
        this.level.text = level.ToString();
        this.date.text = DateTool.Diff(date);
        this.showNotifications = showNotifications;
    }

    public void OnAcceptFriend()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{HttpConst.ACCEPT_FRIEND.Value}/{id}");
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.AddFriend(request, showNotifications));

        Home.instance.GetUserData();
        showNotifications();
    }
}
