using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MainMission", menuName = "MainMission", order = 1)]
public class Mission : ScriptableObject
{
    public int persons = 1;
    public bool firstMission;
    public bool sideMission;

    [Header("Dialog")]
    [TextArea(2, 10)]
    public List<string> missionStartDialog = new List<string>();
    public List<string> missionPickupDialog = new List<string>();
    public List<string> missionEndDialog = new List<string>();
    [Header("Name of Store")]
    public string targetStore;

    [Header("MissionPosition")]
    public int missionTile;

    [Header("Person Information")]
    public string personName;
    public Sprite portret;
}