using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Health
{
    [field: SerializeField] public Slider HealthBarUI;

    [field: SerializeField] public int maxHealthValue { get; private set; } = 100;

    [field: SerializeField] public int healthValue { get; private set; }

    public void Initialize()
    {
        healthValue = 90;
        SetBarValue(HealthBarUI, (float)healthValue / maxHealthValue);
    }


    private void SetBarValue(Slider slider, float val)
    {
        slider.value = val;
    }

    public bool AddHealth(int val)
    {
        //FIXME:

        if (healthValue < maxHealthValue)
        {
            healthValue += val;
            healthValue = Math.Clamp(healthValue, 0, 100);
            SetBarValue(HealthBarUI, (float)healthValue / maxHealthValue);
            Debug.LogFormat("true Health {0}", healthValue);
            return true;
        }
        else
        {
            Debug.LogFormat("false Health {0}", healthValue);
            return false;
        }
    }
}