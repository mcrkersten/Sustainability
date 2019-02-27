using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParts : MonoBehaviour
{
    public StoreList stores;
    public int shipType;
}

[System.Serializable]
public class Parts {
    public List<GameObject> buyableParts;
    public List<int> price;
    public List<bool> bought;
}

[System.Serializable]
public class StoreList {
    public List<Parts> stores;
}