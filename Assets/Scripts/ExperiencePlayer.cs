using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePlayer : MonoBehaviour
{
    public int maxExperience = 100;
    public int currentExperience;

    public ExperienceBar experienceBar;

    void Start()
    {
        currentExperience = maxExperience;
        experienceBar.SetMaxExperience(maxExperience);
    }

    void Update()
    {
        
    }
}
