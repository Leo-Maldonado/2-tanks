using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2BuyButton : MonoBehaviour
{
    public GameObject Panel;

    private TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void OnPurchaseButtonClick()
    {

        if (Panel != null && turnManager.IsPlayerTurn(2) && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            Panel.SetActive(true);
        }
    }
}
