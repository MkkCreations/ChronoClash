using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankPersonTemplate : MonoBehaviour
{
    private string id;
    public RawImage avatar;
    public TMP_Text username;
    public TMP_Text level;
    public TMP_Text rank;
    private Action showPeople;

    public void SetData(string id, string username, string avatar, int level, Action showPeople)
    {
        this.id = id;
        if (avatar.Length != 0)
        {
            this.avatar.texture = ImageTools.CreateTextureFromString(avatar);
        }
        this.username.text = username;
        this.level.text = level.ToString();
        this.rank.text = level.ToString();
        this.showPeople = showPeople;
    }


}
