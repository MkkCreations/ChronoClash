using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogTemplate : MonoBehaviour
{
    public TMP_Text type;
    public TMP_Text description;
    public TMP_Text date;

    public void SetData(string type, string description, DateTime date)
    {
        this.type.text = type;
        this.description.text = description;
        this.date.text = date.ToString("g");
    }
}
