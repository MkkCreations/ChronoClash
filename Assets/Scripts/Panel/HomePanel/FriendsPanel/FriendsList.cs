using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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
        foreach (User.Friend friend in User.instance.user.user.friends)
        {
            GameObject friendInst = Instantiate(FriendTamplate);
            friendInst.transform.SetParent(scrollView.transform);
            friendInst.GetComponent<FriendTemplate>().SetData(friend.id, friend.friend.username, friend.friend.image, friend.friend.level.level);
        }
    }
}
