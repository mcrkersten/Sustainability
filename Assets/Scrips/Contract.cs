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
    public GameObject progressPrefab;
    private ContractCardAvailable availableUI;
    private ContractCardProgress progressUI;
    public GameObject ui;

    public void SetInAvailible()
    {
        ui = GameObject.FindGameObjectWithTag("Available");
        GameObject i = Instantiate(availablePrefab, ui.transform);
        i.transform.Translate(new Vector3(0, -((ContractManager.Instance.currentContract - 1) * 100), 0));
        availableUI = i.GetComponent<ContractCardAvailable>();
        availableUI.c = this;
        availableUI.rewardAmount.text = contractReward.ToString();
        availableUI.contractor.text = contractor;
        availableUI.peopleToCollect.text = personsToCollect.ToString();
        availableUI.button.onClick.AddListener(delegate { SetInProgress(); });
    }

    public void SetInProgress()
    {
        ui = GameObject.FindGameObjectWithTag("Progress");
        GameObject i = Instantiate(progressPrefab, ui.transform);
        i.transform.Translate(new Vector3(0, -((ContractManager.Instance.currentContract - 1) * 100), 0));
        progressUI = i.GetComponent<ContractCardProgress>();
        progressUI.c = this;
        progressUI.rewardAmount.text = contractReward.ToString();
        progressUI.contractor.text = contractor;
        progressUI.peopleToCollect.text = personsToCollect.ToString();
        Ship.Instance.currentContracts.Add(this);
        Destroy(availableUI.gameObject);

        //Set contract in to "inrpogress" in the Contract manager 
        ContractManager.Instance.currentContracts.Add(progressUI.c);
    }
}
