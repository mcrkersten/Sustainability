using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{
    private bool isTalking;
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

    [Header("MissionBriefing")]
    public GameObject mainMissionBoard;
    public TextMeshProUGUI bubbleText;
    public TextMeshProUGUI personName;

    public Image portret;
    public int currentMissionNumber = 0;
    private Mission[] mainMissions;
    public Mission currentMainMission;
    public Mission sideMission;
    public CityPointer missionPointer;

    private bool missionActive;
    private Contract currentContract;

    [HideInInspector]
    public Contract sideContract;

    [HideInInspector]
    private int missionsDone;
    public int minimalSideMissionsForMainMission;

    [Header("MissionInformationPanel")]
    public TextMeshProUGUI contractOrigin;
    public TextMeshProUGUI pickupLocation;
    public TextMeshProUGUI personsToCollect;
    public TextMeshProUGUI reward;
    public GameObject warningIcon;
    public GameObject informationDisplay;
    private Coroutine co;

    private void Awake() {
        InitListners();
        mainMissions = Resources.LoadAll<Mission>("MainMissions");
        mainMissionBoard.SetActive(true);
        currentMainMission = mainMissions[currentMissionNumber];
        OnBubblePress();
    }

    private void Update() {
        if (missionActive) {
            warningIcon.SetActive(true);
        }
        else {
            warningIcon.SetActive(false);
            informationDisplay.SetActive(false);
        }
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
                if (!isTalking) {
                    co = StartCoroutine(ShowText(currentMainMission.missionStartDialog[currentLine]));
                }
                else {
                    StopCoroutine(co);
                    bubbleText.text = currentMainMission.missionStartDialog[currentLine];
                    isTalking = false;
                    currentLine++;
                }
            }
            else {
                currentLine = 0;
                StartMainMission();
                if (Ship.Instance.showMissionPointer) {
                    missionPointer.gameObject.transform.parent.gameObject.SetActive(true);
                }
                missionPointer.target = currentContract.contractPosition;
                mainMissionBoard.SetActive(false);
            }
        }

        //Center of mission (Pickup)
        else if (missionActive && !currentContract.done) {
            if (currentLine < currentMainMission.missionPickupDialog.Count) {
                if (!isTalking) {
                    co = StartCoroutine(ShowText(currentMainMission.missionPickupDialog[currentLine]));
                }
                else {
                    StopCoroutine(co);
                    bubbleText.text = currentMainMission.missionPickupDialog[currentLine];
                    isTalking = false;
                    currentLine++;
                }

            }
            else {
                currentLine = 0;
                missionPointer.target = GameObject.Find(currentContract.contractor);
                mainMissionBoard.SetActive(false);
            }
        }

        //End of mission
        else if (currentContract.done) {
            if (currentLine < currentMainMission.missionEndDialog.Count) {
                if (!isTalking) {
                    co = StartCoroutine(ShowText(currentMainMission.missionEndDialog[currentLine]));
                }
                else {
                    StopCoroutine(co);
                    bubbleText.text = currentMainMission.missionEndDialog[currentLine];
                    isTalking = false;
                    currentLine++;
                }

            }
            else {
                currentLine = 0;
                EndMainMission();
                mainMissionBoard.SetActive(false);
                missionPointer.gameObject.transform.parent.gameObject.SetActive(false);
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
                if (!isTalking) {
                    co = StartCoroutine(ShowText(sideMission.missionStartDialog[currentLine]));
                }
                else {
                    StopCoroutine(co);
                    bubbleText.text = sideMission.missionStartDialog[currentLine];
                    currentLine++;
                    isTalking = false;
                }
            }
            else {
                currentLine = 0;
                ButtonManager.Instance.CloseOpenMissionBoard();
                mainMissionBoard.SetActive(false);
                StartSideMission();
                if (Ship.Instance.showMissionPointer) {
                    missionPointer.gameObject.transform.parent.gameObject.SetActive(true);
                }
                missionPointer.target = sideContract.contractPosition;
                return;
            }
        }

        //Center of mission (Pickup)
        else if (missionActive && !sideContract.done) {
            if (currentLine < sideMission.missionPickupDialog.Count) {
                if (!isTalking) {
                    co = StartCoroutine(ShowText(sideMission.missionPickupDialog[currentLine]));
                }
                else {
                    StopCoroutine(co);
                    bubbleText.text = sideMission.missionPickupDialog[currentLine];
                    currentLine++;
                    isTalking = false;
                }
            }
            else {
                currentLine = 0;
                string t = sideContract.storeName;
                missionPointer.target = GameObject.Find(t);
                mainMissionBoard.SetActive(false);
                sideContract.done = true;
                return;
            }
        }

        //End of mission
        else if (sideContract.done) {
            if (currentLine < sideMission.missionEndDialog.Count) {
                if (!isTalking) {
                    co = StartCoroutine(ShowText(sideMission.missionEndDialog[currentLine]));
                }
                else {
                    StopCoroutine(co);
                    bubbleText.text = sideMission.missionEndDialog[currentLine];
                    currentLine++;
                    isTalking = false;
                }
            }
            else {
                currentLine = 0;
                EndSideMission();
                mainMissionBoard.SetActive(false);
                ButtonManager.Instance.openMenu.Add(ButtonManager.Instance.openStorePromt);
                ButtonManager.Instance.openStorePromt.SetActive(true);
                missionPointer.gameObject.transform.parent.gameObject.SetActive(false);
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

        UpdateMissionInformationPanel(currentMainMission);

        if (currentMainMission.firstMission) {
            currentContract.colectedPersons = 1;
            currentContract.done = true;
            currentContract.contractPosition = GameObject.Find("city[The Capital]");
        }
    }

    public void StartSideMission() {
        missionActive = true;
        UpdateMissionInformationPanel(sideMission);
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

    private void UpdateMissionInformationPanel(Mission m) {
        contractOrigin.text = m.targetStore;
        pickupLocation.text = "Not set or unknown";
        personsToCollect.text = m.persons.ToString();
        if (currentContract != null) {
            reward.text = currentContract.contractReward.ToString();
        }
        if (sideContract != null) {
            reward.text = sideContract.contractReward.ToString();
        }
    }

    IEnumerator ShowText(string text) {
        isTalking = true;
        string currentText = "";
        for (int i = 0; i <= text.Length; i++) {
            currentText = text.Substring(0, i);
            bubbleText.text = currentText;
            yield return new WaitForEndOfFrame();
        }
        currentLine++;
        isTalking = false;
    }
}

