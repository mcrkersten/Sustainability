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
    public GameObject refugee;

    public GameObject contractPosition;

    [Header("Menu")]
    public GameObject availablePrefab;
    public GameObject progressPrefab;
    public GameObject selfInAvailableContractScreen;
    public GameObject selfInActiveContractScreen;
    public ContractCardAvailable availableUI;
    public ContractCardProgress progressUI;
    public ContractCardProgress selfProgressUI;
    public GameObject ui;

    public void SetInAvailible()
    {
        //Get random tile to spawn refugees on
        int random = Random.Range(0, ContractManager.Instance.refSpawner.tiles.Count);
        contractPosition = ContractManager.Instance.refSpawner.tiles[Random.Range(0,ContractManager.Instance.refSpawner.tiles.Count)];

        GameObject i = Instantiate(availablePrefab, ContractManager.Instance.uiContractElements[0].transform);
        i.transform.position = new Vector3(i.transform.position.x, 600, i.transform.position.z);
        selfInAvailableContractScreen = i;
        i.transform.Translate(new Vector3(0, -((ContractManager.Instance.currectPosition++ -1) * 87), 0));
        availableUI = i.GetComponent<ContractCardAvailable>();
        availableUI.c = this;
        availableUI.rewardAmount.text = contractReward.ToString();
        availableUI.contractor.text = contractor;
        availableUI.peopleToCollect.text = personsToCollect.ToString();
        availableUI.button.onClick.AddListener(delegate { SetInProgress(); });
    }

    public void SetInProgress()
    {   if(ContractManager.Instance.currentContracts.Count < 4)
        {
            GameObject i = Instantiate(progressPrefab, ContractManager.Instance.uiContractElements[1].transform);
            i.transform.position = new Vector3(i.transform.position.x, 600, i.transform.position.z);
            selfInAvailableContractScreen = i;
            i.transform.Translate(new Vector3(0, -((ContractManager.Instance.currectPositionInProgress++ - 1) * 87), 0));
            progressUI = i.GetComponent<ContractCardProgress>();
            progressUI.c = this;
            progressUI.rewardAmount.text = contractReward.ToString();
            progressUI.contractor.text = contractor;
            progressUI.peopleToCollect.text = personsToCollect.ToString();
            Ship.Instance.currentContracts.Add(this);

            //Remove contract form available contracts
            ContractManager.Instance.currectPosition--;
            ContractManager.Instance.existingContracts.Remove(this);
            for (int q = 0; q < personsToCollect; q++)
            {
                CreateRefugees();
            }
            Destroy(availableUI.gameObject);

            //Set contract in to "inrpogress" in the Contract manager 
            ContractManager.Instance.currentContracts.Add(progressUI.c);
            SetInProgressScreenOnly(i);
        }
    }

    private void SetInProgressScreenOnly(GameObject s)
    {
        GameObject i = Instantiate(progressPrefab, ContractManager.Instance.uiContractElements[2].transform);
        selfInActiveContractScreen = i;
        i.transform.Translate(new Vector3(0, -((ContractManager.Instance.currentPositionInActiveContracts++) * 87), 0));
        selfProgressUI = i.GetComponent<ContractCardProgress>();
        selfProgressUI.c = this;
        selfProgressUI.rewardAmount.text = contractReward.ToString();
        selfProgressUI.contractor.text = contractor;
        selfProgressUI.peopleToCollect.text = personsToCollect.ToString();
    }

    private void CreateRefugees()
    {
        GameObject r = Instantiate(refugee, contractPosition.transform);
        float size = 24;
        float randomXpos = Random.Range(-size, size);
        float randomZpos = Random.Range(-size, size);
        r.transform.localPosition = new Vector3(randomXpos,-.5f, randomZpos);
        r.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        r.GetComponent<Person>().contract = this;
        r.GetComponent<Person>().portret = ContractManager.Instance.portrets[0];
    }

    public void CreateRefugeesOnPosition(GameObject pos, Person p) {
        float size = 24;
        float randomXpos = Random.Range(-size, size);
        float randomZpos = Random.Range(-size, size);
        p.gameObject.transform.parent = null;
        p.gameObject.transform.localPosition = new Vector3(pos.transform.position.x + 1, -.5f, pos.transform.position.z + 1);
        p.gameObject.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        p.gameObject.SetActive(true);
    }

    public void OnDestroy()
    {
        ContractManager.Instance.portretManager = 0;
        Ship.Instance.currentPersonsOnShip = 0;
        Destroy(selfInActiveContractScreen);
        Destroy(selfInAvailableContractScreen);
        Destroy(this);
    }

    public void ResetPicture()
    {
        for (int i = 0; i < ContractManager.Instance.portUI.portrets.Length; i++)
        {
            ContractManager.Instance.portUI.portrets[i].sprite = ContractManager.Instance.portrets[3];
        }
    }
}
