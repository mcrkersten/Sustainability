using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewModel : MonoBehaviour
{
    private static PreviewModel instance = null;
    public static PreviewModel Instance
    {
        get {
            if (instance == null) {
                // This is where the magic happens.
                instance = FindObjectOfType(typeof(PreviewModel)) as PreviewModel;
            }

            //// If it is still null, create a new instance
            //if (instance == null) {
            //    throw new System.ArgumentException("instance of Previewmodel can not be null", "original");
            //}
            return instance;
        }
    }

    public GameObject[] ship;
    public GameObject instantiatedShip;
    public GameObject parent;
    //public GameObject ship2;
    //public GameObject ship3;

    private void Start() {
        instantiatedShip = Instantiate(ship[Ship.Instance.currentShip], parent.transform);
    }
}


