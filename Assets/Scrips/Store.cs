using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [HideInInspector]
    public bool visited;

    [Header("Shopkeeper Images")]
    public Sprite clerk;
    public Sprite clerkGlow;
    public Sprite storeSymbol;

    [Header("Color of the UI")]
    public Color storeColor;

    [Header("In store Text")]
    public string storeName;
    public string storeSlogan;
    public string clerkName;

    [Header("StoreNumer")]
    public int storeNumber;

    [Header("FuelPrice")]
    public int fuelCostPerUnit;


    public bool isShipStore;

    [Header("Introduction")]
    [TextArea(2, 10)]
    public List<string> speech = new List<string>();
}
