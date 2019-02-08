using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public Sprite portret;
    private Collider col;
    private Rigidbody rb;
    public Contract contract;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<Collider>();
        col.isTrigger = true;
        portret = ContractManager.Instance.portrets[Random.Range(0, ContractManager.Instance.portrets.Length -1)];
    }
}
