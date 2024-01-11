using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInfos : MonoBehaviour
{
    public TMP_Text TMP_Username;
    public TMP_Text TMP_NiveauDynamique;
    public TMP_Text TextExperience;

    // Start is called before the first frame update
    void Start()
    {
        TMP_Username.text = User.instance.user.user.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
