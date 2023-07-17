using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider HealthBarUI;

    [SerializeField]
    private int maxHealthValue = 100;
    public int healthValue { get; private set; }

    void Start()
    {
        healthValue = 90;
        Debug.Log("1111 " + healthValue/maxHealthValue);
        SetBarValue(HealthBarUI, (float)healthValue/maxHealthValue);
    }
    

    private void SetBarValue(Slider slider, float val)
    {
        Debug.Log("val = " + val);
        slider.value = val;
    }

    public bool AddHealth(int val)
    {
        //FIXME:

        if (healthValue < maxHealthValue)
        {
            healthValue += val;
            healthValue = Math.Clamp(healthValue, 0, 100);
            SetBarValue(HealthBarUI, (float)healthValue/maxHealthValue);
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