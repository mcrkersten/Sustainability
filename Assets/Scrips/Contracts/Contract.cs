using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Contract", menuName = "Contract", order = 1)]
public class Contract : ScriptableObject
{
    ContractManager contractManager;
    public int maxContracts;
    public bool done = false;
    public string contractor;
    public int contractNumber;
    public int personsToCollect;
    public int colectedPersons;
    public int contractReward;
    public GameObject refugee;
    public Store store;
    public string storeName;

    public GameObject contractPosition;

    [Header("Menu")]    
    public GameObject progressContractPrefab;

    //prefab
    [Header("Available UI prefab")]
    public GameObject availableContractPrefab;
    //Instantiated prefab
    public GameObject selfInAvailableContractScreen { get; private set; }


    public GameObject selfInActiveContractScreen;

    //Menu contract cards
    public ContractCardAvailable availableUI;
    public ContractCardProgress progressUI;

    public ContractCardProgress selfProgressUI;
    public Mission currentSideMission;

    // public GameObject ui;

    public void Awake() {
        contractManager = ContractManager.Instance;     
    }

    //For menu
    public void SetInAvailible()
    {
        //Get random tile from mission to spawn refugees on
        if(currentSideMission != null) {
            contractPosition = contractManager.refSpawner.tiles[currentSideMission.missionTile];
        }


        //Instantiate UI prefab, selfInAvailableContractScreen
        selfInAvailableContractScreen = Instantiate(availableContractPrefab, contractManager.uiContractElements[0].transform);
        selfInAvailableContractScreen.transform.position = new Vector3(selfInAvailableContractScreen.transform.position.x, 1000, selfInAvailableContractScreen.transform.position.z);
        selfInAvailableContractScreen.transform.Translate(new Vector3(0, -((contractManager.currectPosition++ -1) * 87), 0));
        availableUI = selfInAvailableContractScreen.GetComponent<ContractCardAvailable>();

        //Set variables in ContractCardAvailable script that is on selfInAvailableContractScreen gameObject
        availableUI.c = this;
        availableUI.rewardAmount.text = contractReward.ToString();
        availableUI.contractor.text = contractor;
        if(personsToCollect > 1) {
            availableUI.peopleToCollect.text = personsToCollect.ToString() + " people";
        }
        else {
            availableUI.peopleToCollect.text = personsToCollect.ToString() + " person";
        }
        ContractManager.Instance.UpdateUIPositionsBigMenu();
        //Makes button in UI link to SetInProgress function
        availableUI.button.onClick.AddListener(delegate { SetInProgress(); });
    }

    //Spawns refugees
    public void SetInProgress()
    {
        if (contractManager.currentContracts.Count < maxContracts)
        {
            currentSideMission.targetStore = Ship.Instance.currentStore.gameObject.name;
            storeName = currentSideMission.targetStore;

            MissionManager.Instance.sideMission = this.currentSideMission;
            MissionManager.Instance.sideContract = this;
            MissionManager.Instance.mainMissionBoard.SetActive(true);
            MissionManager.Instance.OnBubblePress();
            ButtonManager.Instance.CloseOpenMissionBoard();

            selfInAvailableContractScreen = Instantiate(progressContractPrefab, contractManager.uiContractElements[1].transform);
            selfInAvailableContractScreen.transform.position = new Vector3(selfInAvailableContractScreen.transform.position.x, 1000, selfInAvailableContractScreen.transform.position.z);
            selfInAvailableContractScreen.transform.Translate(new Vector3(0, -((contractManager.currectPositionInProgress++ - 1) * 87), 0));
            progressUI = selfInAvailableContractScreen.GetComponent<ContractCardProgress>();

            //Set variables in ContractCardAvailable script that is on selfInAvailableContractScreen gameObject
            progressUI.c = this;
            progressUI.rewardAmount.text = contractReward.ToString();
            progressUI.contractor.text = contractor;

            if (personsToCollect > 1) {
                progressUI.peopleToCollect.text = personsToCollect.ToString() + " people";
            }
            else {
                progressUI.peopleToCollect.text = personsToCollect.ToString() + " person";
            }

            Ship.Instance.currentContracts.Add(this);

            //Remove contract form available contracts
            contractManager.currectPosition--;
            contractManager.existingContracts.Remove(this);
            for (int q = 0; q < personsToCollect; q++)
            {
                CreateRefugees();
            }
            Destroy(availableUI.gameObject);

            //Set contract in to "inprogress" in the Contract manager 
            contractManager.currentContracts.Add(progressUI.c);
            SetInProgressScreenOnly(selfInAvailableContractScreen);
        }
        ContractManager.Instance.UpdateUIPositionsBigMenu();
    }

    private void SetInProgressScreenOnly(GameObject s)
    {
        selfInActiveContractScreen = Instantiate(progressContractPrefab, contractManager.uiContractElements[2].transform);
        selfInActiveContractScreen.transform.Translate(new Vector3(0, -((contractManager.currentPositionInActiveContracts++) * 87), 0));

        selfProgressUI = selfInActiveContractScreen.GetComponent<ContractCardProgress>();
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

    public void DestroyContract(bool mainMission) {
        Ship.Instance.currentPersonsOnShip = 0;
        Destroy(selfInActiveContractScreen);
        Destroy(selfInAvailableContractScreen);
        if (!mainMission) {
            contractManager.currectPositionInProgress--;
        }
        Destroy(this);
    }
}
