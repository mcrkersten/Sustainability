using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    public bool canDrop;
    private bool once = false;
    private static Ship instance = null;
    public static Ship Instance
    {
        get
        {
            if (instance == null)
            {
                // This is where the magic happens.
                instance = FindObjectOfType(typeof(Ship)) as Ship;
            }

            // If it is still null, create a new instance
            if (instance == null)
            {
                GameObject i = new GameObject("Ship");
                i.AddComponent(typeof(Ship));
                instance = i.GetComponent<Ship>();
            }
            return instance;
        }
    }

    public List<Contract> currentContracts = new List<Contract>();
    public int currentPersonsOnShip;

    //upgradables
    public int maxPersonsOnShip;
    public float currentFuel;
    public float maxFuel;
    public Slider uiSlider;
    public Slider storeUiSlider;

    public delegate void EnterCity();
    public static event EnterCity OnEnterCity;

    public delegate void ExitCity();
    public static event ExitCity OnExitCity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("City")) {
            OnEnterCity?.Invoke();
        }
        //If gameObject has a personClass on it.
        if (other.gameObject.GetComponent<Person>() != null && !once)
        {
            once = true;
            Person p = other.gameObject.GetComponent<Person>();
            foreach (Contract c in currentContracts)
            {
                if(c.contractNumber == p.contract.contractNumber && currentPersonsOnShip < maxPersonsOnShip)
                {
                    //Person is a part of the contract.
                    c.colectedPersons++;
                    ContractManager.Instance.portUI.portrets[currentPersonsOnShip].sprite = p.portret;
                    currentPersonsOnShip++;
                    ContractManager.Instance.portretManager++;
                    //Contract is done if all persons are collected
                    if (c.personsToCollect == c.colectedPersons)
                    {
                        canDrop = true;
                        c.done = true;
                    }
                    ContractManager.Instance.passangers.Add(p);
                    p.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("City")) {
            OnExitCity?.Invoke();
        }
        once = false;
    }

    private void Start() {
        InitListners();
        uiSlider.maxValue = maxFuel;
        storeUiSlider.maxValue = maxFuel;
    }

    private void InitListners() {
        ButtonManager.OnRefuelShip += Refuel;
    }

    private void Update() {
        uiSlider.value = currentFuel;
        storeUiSlider.value = currentFuel;
    }

    private void Refuel() {
        currentFuel = maxFuel;
    }
}
