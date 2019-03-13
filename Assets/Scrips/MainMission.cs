using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainMission", menuName = "MainMission", order = 1)]
public class MainMission : ScriptableObject
{
    private Contract mission;
    private Ship ship;
    private readonly int persons = 1;

    [Header("Set these for mission")]
    public string[] missionDialog;
    public Store targetStore;

    public void InitContract()
    {
        ship.currentPersonsOnShip = persons;

        mission = Instantiate(ContractManager.Instance.contractBasis);
        mission.store = targetStore;
        mission.personsToCollect = persons;
    }
}