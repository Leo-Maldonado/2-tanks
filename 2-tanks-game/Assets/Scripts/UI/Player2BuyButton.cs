using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2BuyButton : MonoBehaviour
{
    public GameObject Panel;

    private TurnManager turnManager;

    private Button Button;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        Button = this.GetComponent<Button>();
    }

    public void Update()
    {
        if (turnManager.IsPlayerTurn(2) && Button)
        {
            Button.interactable = true;
        }
        else if (turnManager.IsPlayerTurn(1) && Button)
        {
            Button.interactable = false;
        }
    }

    public void OnPurchaseButtonClick()
    {

        if (Panel != null && turnManager.IsPlayerTurn(2) && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            Panel.SetActive(true);
        }
    }
}
