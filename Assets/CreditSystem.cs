using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditSystem : MonoBehaviour
{
    private static CreditSystem instance = null;
    public static CreditSystem Instance
    {
        get
        {
            if (instance == null)
            {
                // This is where the magic happens.
                instance = FindObjectOfType(typeof(CreditSystem)) as CreditSystem;
            }

            // If it is still null, create a new instance
            if (instance == null)
            {
                GameObject i = new GameObject("Credit Manager");
                i.AddComponent(typeof(CreditSystem));
                instance = i.GetComponent<CreditSystem>();
            }
            return instance;
        }
    }

    public int credits;

    public int fuelCost;

    public Text inFlightUiCreditAmount;
    public Text inStoreCreditUiAmount;

    public void Update() {
        inFlightUiCreditAmount.text = credits.ToString();
        inStoreCreditUiAmount.text = credits.ToString();
    }
}
