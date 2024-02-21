using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FriendTemplate : MonoBehaviour
{
    private string id;
    public RawImage avatar;
    public TMP_Text username;
    public TMP_Text level;

    public void SetData(string id, string username, string avatar, int level)
    {
        this.id = id;
        if (avatar.Length != 0)
        {
            this.avatar.texture = ImageTools.CreateTextureFromString(avatar);
        }
        this.username.text = username;
        this.level.text = level.ToString();
    }

    public void OnAcceptFriend()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{HttpConst.ACCEPT_FRIEND.Value}/{id}");
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        //StartCoroutine(Requests.instance.InviteToPrivateGame(request));
    }
}
