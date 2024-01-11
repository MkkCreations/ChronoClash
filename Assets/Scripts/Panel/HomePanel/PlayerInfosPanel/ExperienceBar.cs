using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;
    public Image fil;

    public void SetMaxExperience(int experience)
    {
        slider.maxValue = experience;
        slider.value = experience;

        fil.color = gradient.Evaluate(1f); // Si on a 100 d'xp, on a la couleur la plus à droite du gradient
    }

    public void SetExperience(int experience)
    {
        slider.value = experience;

        fil.color = gradient.Evaluate(slider.normalizedValue); // Si on a 50 d'xp, on a la couleur au milieu du gradient
    }

}

