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

    public int portretManager = 0;
    public Sprite[] portrets;
    public Portrets portUI;
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

    private void Start()
    {
      BackgroundMusicManager bm =  BackgroundMusicManager.GetInstance();
        bm.PlayMusic("Moon Overworld");
    }

    [ContextMenu("Create Contract")]
    //Function creates contracts
    public void CreateContact()
    {
        Contract tempContract = Instantiate(contractBasis);
        tempContract.name = "Contract: " + currentContract;

        //For NonMainMissions.
        existingContracts.Add(tempContract);

        //Set Pramaters
        tempContract.contractNumber = currentContract++;
        tempContract.personsToCollect = Random.Range(1, 4);
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
                Destroy(c);
            }
            existingContracts.Clear();
        }
        for(int i = 0; i < 4; i++)
        {
            CreateContact();
        }
    }

    public void KickRefugee(int number) {
        if(passangers[number] != null) {
            passangers[number].contract.CreateRefugeesOnPosition(Ship.Instance.gameObject, passangers[number]);
            portUI.portrets[number].sprite = portrets[3];
            passangers[number].contract.colectedPersons--;
            passangers.Remove(passangers[number]);
            Ship.Instance.currentPersonsOnShip--;
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
