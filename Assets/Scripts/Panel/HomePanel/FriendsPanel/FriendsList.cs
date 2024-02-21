using UnityEngine;

public class FriendsList : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject FriendTamplate;

    private void OnEnable()
    {
        ShowFriends();
    }

    public void ShowFriends()
    {
        ResetList();
        foreach (User.Friend friend in User.instance.user.user.friends)
        {
            GameObject friendInst = Instantiate(FriendTamplate);
            friendInst.transform.SetParent(scrollView.transform);
            friendInst.GetComponent<FriendTemplate>().SetData(friend.id, friend.friend.username, friend.friend.image, friend.friend.level.level);
        }
    }

    private void ResetList()
    {
        foreach (FriendTemplate friend in scrollView.transform.GetComponentsInChildren<FriendTemplate>())
            Destroy(friend.gameObject);
    }
}
