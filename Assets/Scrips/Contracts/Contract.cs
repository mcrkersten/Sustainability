using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Contract", menuName = "Contract", order = 1)]
public class Contract : ScriptableObject
{
    public bool done;
    public string contractor;
    public int contractNumber;
    public int personsToCollect;
    public int colectedPersons;
    public int contractReward;
    public GameObject availablePrefab;
    public GameObject self;
    public GameObject progressPrefab;
    private ContractCardAvailable availableUI;
    private ContractCardProgress progressUI;
    public GameObject ui;

    public void SetInAvailible()
    {
        GameObject i = Instantiate(availablePrefab, ContractManager.Instance.uiContractElements[0].transform);
        self = i;
        i.transform.Translate(new Vector3(0, -((ContractManager.Instance.currectPosition++) * 75), 0));
        availableUI = i.GetComponent<ContractCardAvailable>();
        availableUI.c = this;
        availableUI.rewardAmount.text = contractReward.ToString();
        availableUI.contractor.text = contractor;
        availableUI.peopleToCollect.text = personsToCollect.ToString();
        availableUI.button.onClick.AddListener(delegate { SetInProgress(); });
    }

    public void SetInProgress()
    {
        GameObject i = Instantiate(availablePrefab, ContractManager.Instance.uiContractElements[1].transform);
        self = i;
        i.transform.Translate(new Vector3(0, -((ContractManager.Instance.currectPositionInProgress++) * 75), 0));
        progressUI = i.GetComponent<ContractCardProgress>();
        progressUI.c = this;
        progressUI.rewardAmount.text = contractReward.ToString();
        progressUI.contractor.text = contractor;
        progressUI.peopleToCollect.text = personsToCollect.ToString();
        Ship.Instance.currentContracts.Add(this);

        //Remove contract form available contracts
        ContractManager.Instance.currectPosition--;
        ContractManager.Instance.existingContracts.Remove(this);
        Destroy(availableUI.gameObject);

        //Set contract in to "inrpogress" in the Contract manager 
        ContractManager.Instance.currentContracts.Add(progressUI.c);
    }
}
