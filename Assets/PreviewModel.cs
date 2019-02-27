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
    public List<GameObject> shipPreviews = new List<GameObject>();
    public GameObject[] ship;
    //public GameObject instantiatedShip;
    public GameObject parent;

    private void Start() {
        for(int i = 0; i < ship.Length; i++) {
            shipPreviews.Add(Instantiate(ship[i], parent.transform));
            shipPreviews[i].SetActive(false);
        }
    }
}


