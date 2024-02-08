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
            switch(PlayerController.me.selectedBuilding.typeEntite)
            {
                case TypeEntite.TERRESTRE:
                    // Enlève les items déjà présents dans la liste
                    foreach (ItemShop child in ShopList.transform.GetComponentsInChildren<ItemShop>())
                        Destroy(child.gameObject);
                    //Parcours le tableau des items du shop et instancie les items correspondant à la catégorie
                    foreach (GameObject item in shopItems)
                        if (item.GetComponent<Unit>().typeEntite == TypeEntite.TERRESTRE) 
                        {
                            // Instancie un prefab de 'ItemShopList' (templateItem) dans le panel 'ShopList'
                            GameObject itemShop = Instantiate(templateItem, Vector3.zero, Quaternion.identity, GameObject.Find("Content").transform);
                            // Charge les données de l'unité dans le prefab
                            itemShop.GetComponent<ItemShop>().SetItem(item);
                        }
                    break;
                case TypeEntite.AERIENNE:
                    break;
                case TypeEntite.MARITIME:
                    break;
            }
        }
    }
}
