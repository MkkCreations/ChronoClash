using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Uidemoscript : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField userfield;

    public void ButtonDemo()
    {
        output.text = userfield.text;
    }

}
    