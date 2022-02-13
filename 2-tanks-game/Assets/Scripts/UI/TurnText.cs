using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnText : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject tank1;

    private TMP_Text TextComponent;

    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        TextComponent = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        tank1 = GameObject.Find("Tank1");
        if(tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2)
        {
            TextComponent.text = "Turn: Tank 2";
        }
        else if(tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank1)
        {
            TextComponent.text = "Turn: Tank 1";
        }
    }
}
