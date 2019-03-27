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

    [Header("For SideMissions Only: Mission City")]
    public bool flora;
    public bool lumen;
    public bool capital;

    [HideInInspector]
    public List<bool> missionCity = new List<bool>();

    [HideInInspector]
    public bool played;

    [Header("Dialog")]
    [TextArea(2, 10)]
    public List<string> missionStartDialog = new List<string>();
    [TextArea(2, 10)]
    public List<string> missionPickupDialog = new List<string>();
    [TextArea(2, 10)]
    public List<string> missionEndDialog = new List<string>();
    [Header("Name of Store")]
    public string targetStore;

    [Header("MissionPosition")]
    public int missionTile;

    [Header("Person Information")]
    public string personName;
    public Sprite portret;

    void OnEnable() {
        played = false;
        missionCity.Clear();
        missionCity.Add(flora);
        missionCity.Add(lumen);
        missionCity.Add(capital);
    }
}