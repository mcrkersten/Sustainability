using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public bool visited;
    public Sprite clerk, clerkGlow, storeSymbol;
    public Color storeColor;
    public string storeName, storeSlogan;
    public int storeNumber;
    public int fuelCostPerUnit;
    public bool isShipStore;
    public List<string> speech = new List<string>();
}
