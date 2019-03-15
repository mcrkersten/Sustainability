using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public RefugeeSpawner refSpawner;
    public Contract contractBasis;
    public GameObject[] uiContractElements;
    public int currentContract = 0;
    public int currectPosition = 0;
    public int currectPositionInProgress = 0;
    public int currentPositionInActiveContracts = 0;
    public List<Contract> existingContracts = new List<Contract>();
    public List<Contract> currentContracts = new List<Contract>();
    public List<Person> passangers = new List<Person>();
    private Mission[] sideMissions;

    private void Start()
    {
        sideMissions = Resources.LoadAll<Mission>("SideMissions");
        BackgroundMusicManager bm =  BackgroundMusicManager.GetInstance();
        bm.PlayMusic("Moon Overworld");
    }

    [ContextMenu("Create Contract")]
    //Function creates contracts
    public void CreateContact()
    {
        Contract tempContract = Instantiate(contractBasis);
        tempContract.name = "Contract: " + currentContract;
        tempContract.store = Ship.Instance.currentStore;
        //For NonMainMissions.
        existingContracts.Add(tempContract);

        //Set Pramaters
        tempContract.contractNumber = currentContract++;

        tempContract.currentSideMission = Instantiate(sideMissions[Random.Range(0, sideMissions.Length)]);
        tempContract.personsToCollect = tempContract.currentSideMission.persons;
        tempContract.contractReward = tempContract.personsToCollect * 100;
        tempContract.SetInAvailible();
    }

    public void Update()
    {
        List<Contract> temp = new List<Contract>();
        foreach(Contract c in currentContracts)
        {
            if(c == null)
            {
                temp.Add(c);
            }
        }
        foreach(Contract c in temp)
        {
            currentContracts.Remove(c);
        }
    }

    public void InitNewContracts()
    {
        if(existingContracts.Count > 1)
        {
            foreach(Contract c in existingContracts)
            {
                c.DestroyContract(false);
            }
            existingContracts.Clear();
        }
        for(int i = 0; i < 4; i++)
        {
            CreateContact();
        }
    }

    public void UpdateUIPositionsBigMenu() {
        int i = 0;
        foreach (Contract g in existingContracts) {
            g.selfInAvailableContractScreen.transform.position = new Vector3(g.selfInAvailableContractScreen.transform.position.x, 650, g.selfInAvailableContractScreen.transform.position.z);
            g.selfInAvailableContractScreen.transform.Translate(new Vector3(0, -((i++) * 90), 0));
        }
        i = 0;
        foreach (Contract a in currentContracts) {
            a.selfInAvailableContractScreen.transform.position = new Vector3(a.selfInAvailableContractScreen.transform.position.x, 650, a.selfInAvailableContractScreen.transform.position.z);
            a.progressUI.collectedPeople.text = a.colectedPersons.ToString();
            a.selfInAvailableContractScreen.transform.Translate(new Vector3(0, -((i++) * 90), 0));
        }
    }
}
