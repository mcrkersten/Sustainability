using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitContractMenu : MonoBehaviour {
    public GameObject ui;

    public void OnButtonPress()
    {
        ui.SetActive(false);
    }
}
