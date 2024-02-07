using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamesList : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject gameTamplate;

    private void Start()
    {
        foreach (User.Game game in User.instance.user.user.games)
        {
            GameObject gmInst = Instantiate(gameTamplate);
            gmInst.transform.SetParent(scrollView.transform);
            gmInst.GetComponent<GameTemplate>().SetData(game.enemy, game.win, DateTime.Parse(game.date));
        }
    }
}
