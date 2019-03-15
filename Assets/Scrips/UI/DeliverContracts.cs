using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverContracts : MonoBehaviour
{
    public void OnButtonPress()
    {
        foreach(Contract c in ContractManager.Instance.currentContracts)
        {
            if(c.colectedPersons == c.personsToCollect)
            {
                CreditSystem.Instance.credits += c.contractReward;
                c.DestroyContract(false);
            }
        }
    }
}
