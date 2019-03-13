using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManager : MonoBehaviour
{
    // Start is called before the first frame update
    string[] mission1;
    Contract contract;
    ContractManager contractManager;

    private void Start()
    {
        contractManager = ContractManager.Instance;
        //contract = BuildStartMission();
    }

    //private Contract BuildStartMission()
    //{
    //    Contract tempContract = Instantiate(contractBasis);
    //    tempContract.name = "Contract: " + currentContract;

    //    For NonMainMissions.
    //    existingContracts.Add(tempContract);

    //    Set Pramaters
    //    tempContract.contractNumber = currentContract++;
    //    tempContract.personsToCollect = Random.Range(1, 4);
    //    tempContract.contractReward = tempContract.personsToCollect * 100;
    //    tempContract.SetInAvailible();

    //    return tempContract;
    //}
}
