using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums.TypeEntite;

public class ShopListPanel : MonoBehaviour
{
    public GameObject ShopList;
    public static ShopListPanel instance;
    public GameObject[] shopItems;
    public GameObject templateItem;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void UpdateList()
    {
        if (PlayerController.me.selectedBuilding != null && PlayerController.me.selectedBuilding.typeEntite != TypeEntite.HQ)
        {
            // Enl�ve les items d�j� pr�sents dans la liste
            foreach (ItemShop child in ShopList.transform.GetComponentsInChildren<ItemShop>())
                Destroy(child.gameObject);

            switch (PlayerController.me.selectedBuilding.typeEntite)
            {
                case TypeEntite.TERRESTRE:
                    //Parcours le tableau des items du shop et instancie les items correspondant � la cat�gorie
                    foreach (GameObject item in shopItems)
                        if (item.GetComponent<Unit>().typeEntite == TypeEntite.TERRESTRE)
                            this.InstanciatePrefab(item);
                    break;
                case TypeEntite.AERIENNE:
                    foreach (GameObject item in shopItems)
                        if (item.GetComponent<Unit>().typeEntite == TypeEntite.AERIENNE)
                            this.InstanciatePrefab(item);
                    break;
                case TypeEntite.MARITIME:
                    foreach (GameObject item in shopItems)
                        if (item.GetComponent<Unit>().typeEntite == TypeEntite.MARITIME)
                            this.InstanciatePrefab(item);
                    break;
            }
        }
    }

    private void InstanciatePrefab(GameObject item) {
        // Instancie un prefab de 'ItemShopList' (templateItem) dans le panel 'ShopList'
        GameObject itemShop = Instantiate(templateItem, Vector3.zero, Quaternion.identity, GameObject.Find("Content").transform);
        // Charge les donn�es de l'unit� dans le prefab
        itemShop.GetComponent<ItemShop>().SetItem(item);
    }
}
