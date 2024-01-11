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

    
    public ExperienceBar experienceBar;
    private const int MAX_EXPERIENCE = 100;

    // Update is called once per frame
    void Update()
    {
        TMP_Username.text = User.instance.user.user.name;
        experienceBar.SetMaxExperience(MAX_EXPERIENCE);
        experienceBar.SetExperience(User.instance.user.user.level.xp);
    }
}
