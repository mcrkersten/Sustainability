using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{
    private static MissionManager instance = null;
    public static MissionManager Instance
    {
        get {
            if (instance == null) {
                // This is where the magic happens.
                instance = FindObjectOfType(typeof(MissionManager)) as MissionManager;
            }
            return instance;
        }
    }
    private int currentLine = 0;
    public GameObject mainMissionBoard;

    public TextMeshProUGUI bubbleText;
    public TextMeshProUGUI personName;

    public Image portret;
    public int currentMissionNumber = 0;
    private Mission[] mainMissions;
    public Mission currentMainMission;

    public Mission sideMission;

    private bool missionActive;
    private Contract currentContract;

    [HideInInspector]
    public Contract sideContract;

    [HideInInspector]
    private int missionsDone;
    public int minimalSideMissionsForMainMission;

    private void Awake() {
        InitListners();
        mainMissions = Resources.LoadAll<Mission>("MainMissions");
        mainMissionBoard.SetActive(true);
        currentMainMission = mainMissions[currentMissionNumber];
        OnBubblePress();
    }

    private void InitListners() {
        Ship.OnEnterCity += EnterCity;
    }

    public void OnBubblePress()
    {
        if(sideMission == null) {
            OnBubblePressMainMission();
        }
        else {
            OnBubblePressSideMission();
        }
    }

    private void UpdatePerson() {
        if (sideMission == null) {
            portret.sprite = currentMainMission.portret;
            personName.text = currentMainMission.personName;
        }
        else {
            portret.sprite = sideMission.portret;
            personName.text = sideMission.personName;
        }
        
    }

    private void OnBubblePressMainMission() {
        //Start of mission
        UpdatePerson();
        if (!missionActive) {
            if (currentLine < currentMainMission.missionStartDialog.Count) {
                StartCoroutine(ShowText(currentMainMission.missionStartDialog[currentLine]));
                currentLine++;
            }
            else {
                currentLine = 0;
                StartMainMission();
                mainMissionBoard.SetActive(false);
            }
        }

        //Center of mission (Pickup)
        else if (missionActive && !currentContract.done) {
            if (currentLine < currentMainMission.missionPickupDialog.Count) {
                StartCoroutine(ShowText(currentMainMission.missionPickupDialog[currentLine]));
                currentLine++;

            }
            else {
                currentLine = 0;
                mainMissionBoard.SetActive(false);
            }
        }

        //End of mission
        else if (currentContract.done) {
            if (currentLine < currentMainMission.missionEndDialog.Count) {
                StartCoroutine(ShowText(currentMainMission.missionEndDialog[currentLine]));
                currentLine++;

            }
            else {
                currentLine = 0;
                EndMainMission();
                mainMissionBoard.SetActive(false);
                ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
                ButtonManager.Instance.openStorePromt.SetActive(true);
            }
        }
        else {
            ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
            ButtonManager.Instance.openStorePromt.SetActive(true);
        }
    }

    private void OnBubblePressSideMission() {
        //Start of mission
        UpdatePerson();
        if (!missionActive) {
            if (currentLine < sideMission.missionStartDialog.Count && !sideContract.done) {
                StartCoroutine(ShowText(sideMission.missionStartDialog[currentLine]));
                currentLine++;

            }
            else {
                currentLine = 0;
                ButtonManager.Instance.CloseOpenMissionBoard();
                mainMissionBoard.SetActive(false);
                StartSideMission();
                return;
            }
        }

        //Center of mission (Pickup)
        else if (missionActive && !sideContract.done) {
            if (currentLine < sideMission.missionPickupDialog.Count) {
                StartCoroutine(ShowText(sideMission.missionPickupDialog[currentLine]));
                currentLine++;

            }
            else {
                currentLine = 0;
                mainMissionBoard.SetActive(false);
                sideContract.done = true;
                return;
            }
        }

        //End of mission
        else if (sideContract.done) {
            if (currentLine < sideMission.missionEndDialog.Count) {
                StartCoroutine(ShowText(sideMission.missionEndDialog[currentLine]));
                currentLine++;

            }
            else {
                currentLine = 0;
                EndSideMission();
                mainMissionBoard.SetActive(false);
                ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
                ButtonManager.Instance.openStorePromt.SetActive(true);
                return;
            }
        }
        else {
            ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
            ButtonManager.Instance.openStorePromt.SetActive(true);
        }
    }

    private void StartMainMission() {
        missionActive = true;
        currentContract = Instantiate(ContractManager.Instance.contractBasis);
        currentContract.personsToCollect = currentMainMission.persons;
        currentContract.storeName = currentMainMission.targetStore;

        if (currentMainMission.firstMission) {
            currentContract.colectedPersons = 1;
            currentContract.done = true;
        }
    }

    public void StartSideMission() {
        missionActive = true;
    }

    private void EndMainMission() {
        missionActive = false;
        currentContract.DestroyContract(true);
        currentContract = null;
        currentMainMission = null;
        currentMissionNumber++;
    }

    private void EndSideMission() {
        missionActive = false;

        foreach (Contract c in ContractManager.Instance.currentContracts) {
            if (c.colectedPersons == c.personsToCollect) {
                CreditSystem.Instance.credits += c.contractReward;
                c.DestroyContract(false);
            }
        }
        missionsDone++;
        sideContract = null;
        sideMission = null;

        if(missionsDone == minimalSideMissionsForMainMission) {
            mainMissionBoard.SetActive(true);
            currentMainMission = mainMissions[currentMissionNumber];
            missionsDone = 0;
            OnBubblePress();
        }
    }

    private void EnterCity(Store store)
    {
        print(store.gameObject.name);
        if(currentContract != null) {
            if (store.gameObject.name == currentMainMission.targetStore && currentContract.done == true) {
                mainMissionBoard.SetActive(true);
                OnBubblePress();
                return;
            }
            else {
                ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
                ButtonManager.Instance.openStorePromt.SetActive(true);
            }
        }

        if (sideContract != null) {
            print("Succes");
            if (store.gameObject.name == sideMission.targetStore && sideContract.done == true) {
                mainMissionBoard.SetActive(true);
                OnBubblePress();
            }
            else {
                ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
                ButtonManager.Instance.openStorePromt.SetActive(true);
            }
        }
        else {
            ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
            ButtonManager.Instance.openStorePromt.SetActive(true);
        }
    }

    IEnumerator ShowText(string text) {
        string currentText = "";
        for (int i = 0; i <= text.Length; i++) {
            currentText = text.Substring(0, i);
            bubbleText.text = currentText;
            yield return new WaitForEndOfFrame();
        }
    }
}

