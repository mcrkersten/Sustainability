using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDiegeticFeedbackController : MonoBehaviour
{
    int fuelFixCount = 6;

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

    private void OnValidate()
    {
        //Adjust bool[] to size
        if (!Application.isPlaying)
        {
            SetFuelLevel(fuelAmount);
        }
    }

    public void SetFuelLevel(float fuel)
    {
        fuelRend.sharedMaterial.SetFloat("_FillAmount", Remap(fuelAmount, 0, 1.0f, -1.221f, - 2f));
    }


    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void Update()
    {
        float percentage;
        float max = Ship.Instance.maxFuel;
        float cur = Ship.Instance.currentFuel;
        percentage = cur / max;
        SetFuelLevel(percentage);
        fuelAmount = percentage;

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
