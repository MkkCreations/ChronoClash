using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Enums.TypeEntite;

public class ItemShop : MonoBehaviour
{
    public Image itemImage;
    public Text itemDescriptionText;
    public Text itemPriceText;
    public Button recruitButton;
    public GameObject prefabUnit;

    public void onClickRecruitButton()
    {
        if(PlayerController.me.BuyItem(prefabUnit)) {
            Console.WriteLine("OnClickRecruitButton : ITEM ACHETÉ");
        } 
        else
        {
            Console.WriteLine("OnClickRecruitButton : PAS ASSEZ D'ARGENT");
        }

        // ferme le panel
        ShopListPanel.instance.ShopList.SetActive(false);
    }

    public void SetItem(GameObject prefabUnit)
    {
        this.prefabUnit = prefabUnit;
        itemImage.sprite = prefabUnit.GetComponent<Unit>().spriteVisual.sprite;
        // rotation sur z de 90 pour les unités aériennes
        if (prefabUnit.GetComponent<Unit>().typeEntite == TypeEntite.AERIENNE)
            itemImage.transform.rotation = Quaternion.Euler(0, 0, 90);
        itemDescriptionText.text = string.Format("{0}", prefabUnit.name);
        itemPriceText.text = string.Format("{0}", prefabUnit.GetComponent<Unit>().cost);
    }
}

