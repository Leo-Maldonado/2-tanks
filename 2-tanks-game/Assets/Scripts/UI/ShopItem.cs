using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    // The buy menu
    private BuyMenu buyMenu;

    // Start is called before the first frame update
    void Start()
    {
        buyMenu = GameObject.Find("BuyMenu").GetComponent<BuyMenu>();
    }

    // When mouse hovers over button
    public void OnPointerEnter(PointerEventData eventData)
    {
        buyMenu.EnterToolTip(eventData.pointerEnter.transform.parent.gameObject.name);
    }

    // When mouse exits hover
    public void OnPointerExit(PointerEventData eventData)
    {
        buyMenu.ExitToolTip();
    }
}
