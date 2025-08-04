using System.Collections;
using UnityEngine;
using TMPro;

public class FlashLight : MonoBehaviour
{
    public Light flashLight;
    public float maxBatteryLife = 20f;
    public float currentBatteryLife;
    public float batteryDrainRate = 1f;
    public TMP_Text batteryLifeText;


    void Start()
    {
        if (flashLight == null)
        {
            flashLight = GetComponent<Light>();
        }
        currentBatteryLife = maxBatteryLife;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (flashLight != null)
            {
                if(!flashLight.enabled && currentBatteryLife > 0)
                {

                    flashLight.enabled  = true;
                }
                else
                {

                    flashLight.enabled = false;
                }
            }
        }
        if (flashLight != null && flashLight.enabled)
        {
            // Drain battery over time
            currentBatteryLife -= batteryDrainRate * Time.deltaTime;

            if (currentBatteryLife <= 0f)
            {
                currentBatteryLife = 0f;
                flashLight.enabled = false;
            }
        }
        if (batteryLifeText != null)
        {
            float batteryPercentage = (currentBatteryLife / maxBatteryLife) * 100f;
            batteryLifeText.text = "Battery: " + Mathf.CeilToInt(batteryPercentage) + "%";
        }
    }
}

