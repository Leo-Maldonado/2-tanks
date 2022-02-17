using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnText : MonoBehaviour
{
    private TurnManager turnManager;

    private TMP_Text TextComponent;

    void Start()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
        TextComponent = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
        if (turnManager.IsPlayerTurn(2))
        {
            TextComponent.text = "Turn: Tank 2";
        }
        else if(turnManager.IsPlayerTurn(1))
        {
            TextComponent.text = "Turn: Tank 1";
        }
    }
}
