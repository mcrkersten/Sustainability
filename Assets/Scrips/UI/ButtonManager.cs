using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject openStorePromt;

    public GameObject activeContracts;
    public GameObject deliverContract;
    private List<GameObject> openMenu = new List<GameObject>();

    public delegate void RefuelShip();
    public static event RefuelShip OnRefuelShip;

    private void Start()
    {
        InitListners();
        ContractManager.Instance.InitNewContracts();
    }

    private void InitListners() {
        Ship.OnEnterCity += EnterCity;
        Ship.OnExitCity += ExitCity;
    }

    private void EnterCity() {
        openStorePromt.SetActive(true);
    }

    private void ExitCity() {
        ContractManager.Instance.InitNewContracts();
        openStorePromt.SetActive(false);
        foreach (GameObject x in openMenu) {
            x.SetActive(false);
        }
        openMenu.Clear();
    }

    public void ExitMenu(GameObject gameObjectToExit) {
        gameObjectToExit.SetActive(false);
        if (openMenu.Contains(gameObjectToExit)) {
            openMenu.Remove(gameObjectToExit);
        }
    }

    public void OpenMenu(GameObject gameObjectToOpen) {
        gameObjectToOpen.SetActive(true);
        openMenu.Add(gameObjectToOpen);
        UpdateUIPositionsBigMenu();
    }

    private void UpdateUIPositionsBigMenu()
    {
        int i = 0;
        foreach (Contract g in ContractManager.Instance.existingContracts)
        {
            g.selfInAvailableContractScreen.transform.position = new Vector3(g.selfInAvailableContractScreen.transform.position.x, 700, g.selfInAvailableContractScreen.transform.position.z);
            g.selfInAvailableContractScreen.transform.Translate(new Vector3(0, -((i++) * 90), 0));
        }
        i = 0;
        foreach (Contract a in ContractManager.Instance.currentContracts)
        {
            a.selfInAvailableContractScreen.transform.position = new Vector3(a.selfInAvailableContractScreen.transform.position.x, 700, a.selfInAvailableContractScreen.transform.position.z);
            a.progressUI.collectedPeople.text = a.colectedPersons.ToString();
            a.selfInAvailableContractScreen.transform.Translate(new Vector3(0, -((i++) * 90), 0));
        }
    }

    public void FuelShip() {
        if (CreditSystem.Instance.credits > CreditSystem.Instance.fuelCost) {
            CreditSystem.Instance.credits -= CreditSystem.Instance.fuelCost;
            OnRefuelShip?.Invoke();
        }
    }

    public void UpdateUIPositionsSmallMenu() {
        int i = 0;
        foreach (Contract a in ContractManager.Instance.currentContracts) {
            a.selfInActiveContractScreen.transform.position = new Vector3(a.selfInActiveContractScreen.transform.position.x, 700, a.selfInActiveContractScreen.transform.position.z);
            a.selfInActiveContractScreen.transform.Translate(new Vector3(0, -((i++) * 90), 0));
            a.selfProgressUI.collectedPeople.text = a.colectedPersons.ToString();
        }
    }
}
