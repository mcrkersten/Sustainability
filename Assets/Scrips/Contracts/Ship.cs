using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public bool canDrop;
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
    public int maxPersonsOnShip;

    private void OnTriggerEnter(Collider other)
    {
        //If gameObject has a personClass on it.
        if(other.gameObject.GetComponent<Person>() != null)
        {
            canDrop = true;
            Person p = other.gameObject.GetComponent<Person>();
            foreach (Contract c in currentContracts)
            {
                if(c.contractNumber == p.contract.contractNumber && currentPersonsOnShip < maxPersonsOnShip)
                {
                    //Person is a part of the contract.
                    c.colectedPersons++;
                    currentPersonsOnShip++;

                    ContractManager.Instance.portUI.portrets[currentPersonsOnShip -1].sprite = p.portret;
                    ContractManager.Instance.portretManager++;
                    //Contract is done if all persons are collected
                    if (c.personsToCollect == c.colectedPersons)
                    {
                        c.done = true;
                    }
                    Destroy(p.gameObject);                  
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canDrop = false;
    }
}
