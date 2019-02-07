using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    private static ContractManager instance = null;
    public static ContractManager Instance
    {
        get
        {
            if (instance == null)
            {
                // This is where the magic happens.
                instance = FindObjectOfType(typeof(ContractManager)) as ContractManager;
            }

            // If it is still null, create a new instance
            if (instance == null)
            {
                GameObject i = new GameObject("Contract Manager");
                i.AddComponent(typeof(ContractManager));
                instance = i.GetComponent<ContractManager>();
            }
            return instance;
        }
    }
    public Contract contractBasis;
    public GameObject[] uiContractElements;
    public int currentContract = 0;
    public int currectPosition = 0;
    public int currectPositionInProgress = 0;
    public List<Contract> existingContracts = new List<Contract>();
    public List<Contract> currentContracts = new List<Contract>();


    [ContextMenu("Create Contract")]
    //Function creates contracts
    public void CreateContact()
    {
        Contract tempContract = Instantiate(contractBasis);
        tempContract.name = "Contract: " + currentContract;
        existingContracts.Add(tempContract);

        //Set Pramaters
        tempContract.contractNumber = currentContract++;
        tempContract.personsToCollect = Random.Range(1, 4);
        tempContract.contractReward = tempContract.personsToCollect * 100;
        tempContract.SetInAvailible();
    }
}
