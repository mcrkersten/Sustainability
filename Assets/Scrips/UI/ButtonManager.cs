using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private static ButtonManager instance = null;
    public static ButtonManager Instance
    {
        get {
            if (instance == null) {
                // This is where the magic happens.
                instance = FindObjectOfType(typeof(ButtonManager)) as ButtonManager;
            }
            return instance;
        }
    }

    public CanvasColors c;
    public Color ownedColor;
    public GameObject openStorePromt;
    public GameObject activeContracts;
    public GameObject deliverContract;

    [Header("StorePromts")]
    public GameObject tablet;
    public GameObject speechBubble;
    public GameObject storePage;

    [Header("MissionPromt")]
    public GameObject speechBubbleMission;
    public GameObject missionPage;

    public List<GameObject> openMenu = new List<GameObject>();
    private int itemSelected = 0;
    private int storeNumber;
    private Store store;

    [Header(" ")]
    public PreviewModel previewModel;
    public GameObject[] storeFrames;
    private int reFuelPrice;
    private int currentLine = 0;

    public delegate void RefuelShip();
    public static event RefuelShip OnRefuelShip;

    public delegate void BuyItemEvent(GameObject item, bool isShipStore);
    public static event BuyItemEvent OnItemBuy;


    private void Start()
    {
        InitListners();
        ContractManager.Instance.InitNewContracts();
        
    }

    private void InitListners() {
        Ship.OnEnterCity += EnterCity;
        Ship.OnExitCity += ExitCity;
    }

    private void EnterCity(Store s) {
        Ship.Instance.gameObject.GetComponent<PreviewModel>().shipPreviews[Ship.Instance.currentShip].SetActive(true);
        Ship.Instance.currentStore = s;
        store = s; 
        //openStorePromt.SetActive(true);
        //c = this.gameObject.GetComponent<CanvasColors>();
        
        storeNumber = store.storeNumber;
        c.clerk.sprite = store.clerk;
        c.clerkGlow.sprite = store.clerkGlow;
        c.storeSymbol.sprite = store.storeSymbol;
        c.storeSlogan.text = store.storeSlogan;
        c.storeName.text = store.storeName;
        c.unitFuelPrice.text = "Per Unit: " + store.fuelCostPerUnit.ToString() + ",-";
        c.shopKeeperName.text = store.clerkName;
        store.speech = store.speech.Count > 0 ? store.speech : new List<string>(new string[] { "I DONT HAVE TEXT PLEASE HELP", "help me..." });
        c.speechText.text = store.speech[0];
        currentLine = 0;

        for (int i = 0; i < c.textColor.Length; i++) {
            c.textColor[i].color = store.storeColor;
        }
        for (int i = 0; i < c.imageColor.Length; i++) {
            c.imageColor[i].color = store.storeColor;
        }
        UpdateStore();
    }

    public void NextLineOfText() {
        if (currentLine < store.speech.Count) {
            StartCoroutine(ShowText(store.speech[currentLine]));
            currentLine++;

        }
        else {
            tablet.SetActive(true);
            speechBubble.SetActive(false);
            store.visited = true;
        }
    }

    public void CloseOpenMissionBoard() {
        foreach(GameObject i in openMenu) {
            if (i.activeSelf) {
                i.SetActive(false);
            }
            else {
                i.SetActive(true);
            }
        }
    }



    private void UpdateStore() {
        previewModel = PreviewModel.Instance;
        float cost = Ship.Instance.maxFuel - Ship.Instance.currentFuel;
        reFuelPrice = Mathf.RoundToInt((store.fuelCostPerUnit * cost));
        c.fuelCost.text = "Total: " + reFuelPrice.ToString()+",-";
        for (int i = 0; i < storeFrames.Length; i++) {
            storeFrames[i].SetActive(false);
        }

        for (int i = 0; i < previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts.Count; i++) {
            storeFrames[i].SetActive(true);
            if (previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].bought[i]) {
                storeFrames[i].GetComponent<Image>().color = ownedColor;
                storeFrames[i].GetComponent<Button>().enabled = false;
            }
            else {
                storeFrames[i].GetComponent<Button>().enabled = true;
            }
        }

        int z = 0;
        foreach (int x in previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].price) {
            if (!previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].bought[z]) {
                c.storePrices[z].text = x.ToString() + ",-";
            }
            else {
                c.storePrices[z].text = "Owned";
            }
            z++;
        }
    }

    public void OpenStorePage()
    {
        if (!store.visited)
        {
            tablet.SetActive(false);
            storePage.SetActive(true);
            openMenu.Add(storePage);
            speechBubble.SetActive(true);
            StartCoroutine(ShowText(store.speech[currentLine]));
            currentLine++;
        }
        else
        {
            tablet.SetActive(true);
            storePage.SetActive(true);
            speechBubble.SetActive(false);
        }
    }

    private void ExitCity() {
        ContractManager.Instance.InitNewContracts();

        openStorePromt.SetActive(false);
        foreach (GameObject x in openMenu) {
            x.SetActive(false);
        }
        openMenu.Clear();
        itemSelected = 0;
    }

    public void ExitMenu(GameObject gameObjectToExit) {
        gameObjectToExit.SetActive(false);
        if (openMenu.Contains(gameObjectToExit)) {
            openMenu.Remove(gameObjectToExit);
        }
    }

    public void OpenMenu(GameObject gameObjectToOpen) {
        gameObjectToOpen.SetActive(true);
        openMenu.Add(gameObjectToOpen);
        ContractManager.Instance.UpdateUIPositionsBigMenu();
    }

    public void OpenOrCloseMenu(GameObject gameObjectToOpen) {
        if (gameObjectToOpen.activeSelf) {
            gameObjectToOpen.SetActive(false);
        }
        else {
            gameObjectToOpen.SetActive(true);
        }
    }

    public void FuelShip() {
        if (CreditSystem.Instance.credits > reFuelPrice) {
            CreditSystem.Instance.credits -= reFuelPrice;
            OnRefuelShip?.Invoke();
            UpdateStore();
        }
        else {
            //TO:DO NOT ENOUGH CREDITS
        }
    }

    public void UpdateUIPositionsSmallMenu() {
        int i = 0;
        foreach (Contract a in ContractManager.Instance.currentContracts) {
            a.selfInActiveContractScreen.transform.position = new Vector3(a.selfInActiveContractScreen.transform.position.x, 700, a.selfInActiveContractScreen.transform.position.z);
            a.selfInActiveContractScreen.transform.Translate(new Vector3(0, -((i++) * 90), 0));
            a.selfProgressUI.collectedPeople.text = a.colectedPersons.ToString();
        }
    }

    public void SelectStoreItem(int itemNumber) {
        itemSelected = itemNumber;
        if (store.isShipStore) {
            foreach(GameObject x in Ship.Instance.gameObject.GetComponent<PreviewModel>().shipPreviews) {
                x.SetActive(false);
            }
            Ship.Instance.gameObject.GetComponent<PreviewModel>().shipPreviews[itemSelected].SetActive(true);
        }
        else {

            //if selevted object is on, disable it, else turn it on.
            if (!previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts[itemSelected].activeSelf) {
                storeFrames[itemSelected].GetComponent<Image>().color = new Color(255, 0, 0);
                previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts[itemSelected].SetActive(true);
            }
            else {
                storeFrames[itemSelected].GetComponent<Image>().color = store.storeColor;
                previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts[itemSelected].SetActive(false);
            }

            //disable all other objects.
            for (int i = 0; i < PreviewModel.Instance.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts.Count; i++) {
                if (i != itemSelected && !previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].bought[i]) {
                    storeFrames[i].GetComponent<Image>().color = store.storeColor;
                    previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts[i].SetActive(false);
                }
            }
        }
    }

    public void BuyItem() {
        if (!previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].bought[itemSelected]) {
            if (store.isShipStore) {
                Ship.Instance.currentShipMesh.SetActive(false);
                OnItemBuy(previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts[itemSelected], true);
                previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].bought[itemSelected] = true;
            }
            else {
                OnItemBuy(previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].buyableParts[itemSelected], false);
                previewModel.shipPreviews[Ship.Instance.currentShip].GetComponent<ShipParts>().stores.stores[store.storeNumber].bought[itemSelected] = true;
            }
        }
        UpdateStore();
    }

    IEnumerator ShowText(string text) {
        string currentText = "";
        for (int i = 0; i <= text.Length; i++) {
            currentText = text.Substring(0, i);
            c.speechText.text = currentText;
            yield return new WaitForEndOfFrame();
        }
    }
}
