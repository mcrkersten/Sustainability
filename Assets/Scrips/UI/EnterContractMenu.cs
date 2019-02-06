using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterContractMenu : MonoBehaviour
{
    public GameObject uiButton;
    public GameObject contractsUi;
    private bool canOpen;

    private void Update() {
        if(canOpen && Input.GetKeyDown(KeyCode.C)) {
            OpenContracts();
        }
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("City")) {
            canOpen = true;
            uiButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("City")) {
            uiButton.SetActive(false);
        }
    }

    public void OpenContracts() {
        if (uiButton.activeSelf) {
            UpdateUIPositions();
            uiButton.SetActive(false);
            contractsUi.SetActive(true);
        }
        else {
            uiButton.SetActive(true);
            contractsUi.SetActive(false);
        }        
    }

    private void UpdateUIPositions()
    {
        int i = 0;
        foreach(Contract g in ContractManager.Instance.existingContracts) {
            g.self.transform.position = new Vector3(g.self.transform.position.x, 500, g.self.transform.position.z);
            g.self.transform.Translate(new Vector3(0, -((i++) * 75), 0));
        }
        i = 0;
        foreach (Contract a in ContractManager.Instance.currentContracts) {
            a.self.transform.position = new Vector3(a.self.transform.position.x, 500, a.self.transform.position.z);
            a.self.transform.Translate(new Vector3(0, -((i++) * 75), 0));
        }
    }
}
