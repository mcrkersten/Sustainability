using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDiegeticFeedbackController : MonoBehaviour
{
    int fuelFixCount = 6;

    [SerializeField]
    private Renderer[] lightRend;
    public bool[] lightSwitch;

    [SerializeField]
    private Renderer fuelRend;
    [SerializeField]
    private GameObject tankWobbly;

    [Range ( 0f, 1f )]
    public float fuelAmount = 1.0f;

    private void Awake()
    {

        if (tankWobbly != null)
        {
            var c = tankWobbly.GetComponent<Wobble>();
            c.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(var i = 0; i < lightSwitch.Length; i++)
        {
            SwitchLight(i, lightSwitch[i]);
        }


    }

    private void OnValidate()
    {
        //Adjust bool[] to size
        if (!Application.isPlaying)
        {
            bool[] copy = lightSwitch;
            lightSwitch = new bool[lightRend.Length];
            for (var i = 0; i < lightSwitch.Length; i++)
            {
                if (i < copy.Length)
                    lightSwitch[i] = copy[i];
            }

            SetFuelLevel(fuelAmount);
        }
        //Disable for implementation???
        else
        {
            for (var i = 0; i < lightSwitch.Length; i++)
            {
                SwitchLight(i, lightSwitch[i]);
            }
            SetFuelLevel(fuelAmount);
        }
    }

    public void SwitchLight(int light, bool onOff)
    {
        if (light < lightSwitch.Length)
        {
            if (onOff)
                lightRend[light].material.EnableKeyword("_EMISSION");
            else
                lightRend[light].material.DisableKeyword("_EMISSION");

        }
    }

    public void SetFuelLevel(float fuel)
    {
        fuelRend.sharedMaterial.SetFloat("_FillAmount", Remap(fuelAmount,0.0f,1.0f, -0.64f, - 1.14f));
    }


    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void Update()
    {
        //Fixes the issue where Wobble causes the liquid to disappear
        if (fuelFixCount < 0)
        {
            if (tankWobbly != null)
            {
                var c = tankWobbly.GetComponent<Wobble>();
                c.enabled = true;
            }
        }
        else
            fuelFixCount--;
    }
}
