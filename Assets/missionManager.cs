using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public GameObject mainMissionBoard;
    public TextMeshProUGUI bubbleText;
    public int currentMission;
    public MainMission[] mainMissions;

    private void Start()
    {
        mainMissionBoard.SetActive(true);
    }

    public void OnBubblePress()
    {

    }

    private void InitListners()
    {
        Ship.OnEnterCity += EnterCity;
    }

    private void EnterCity(Store store)
    {
        if(mainMissions[currentMission].targetStore == store)
        {
            CompleteMission();
        }
    }

    private void CompleteMission()
    {

    }
}

