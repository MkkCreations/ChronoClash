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
        TMP_Username.text = GameObject.FindObjectOfType<User>().GetComponent<User>().user.user.name;
        TMP_NiveauDynamique.text = GameObject.FindObjectOfType<User>().GetComponent<User>().user.user.level.level.ToString();

        int level = GameObject.FindObjectOfType<User>().GetComponent<User>().user.user.level.level;
        int xp = GameObject.FindObjectOfType<User>().GetComponent<User>().user.user.level.xp;

        // (level.lvl * 100) + level.xp / (level.lvl + 1) * 100
        if (level == 1) TextExperience.text = xp.ToString() + "/" + ((level) * 100).ToString();
        else TextExperience.text = ((level * 100) + xp).ToString() + "/" + ((level) * 100).ToString();

        experienceBar.SetMaxExperience(MAX_EXPERIENCE);
        experienceBar.SetExperience(GameObject.FindObjectOfType<User>().GetComponent<User>().user.user.level.xp);
    }
}
