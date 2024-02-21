using System;
using UnityEngine;
using TMPro;

public class GameTemplate : MonoBehaviour
{
    public TMP_Text enemy;
    public TMP_Text win;
    public TMP_Text date;

    public void SetData(string enemy, int win, DateTime date)
    {
        this.enemy.text = enemy;
        this.win.text = $"You {((win == 1) ? "Won" : "Losed")} against {enemy}";
        this.date.text = DateTool.Diff(date);
    }

}
