using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player2TurnPoints : MonoBehaviour
{
    // Tank
    private GameObject tank2;

    // Text
    private TMP_Text TextComponent;

    // Tank's turn points
    private float tank2TurnPoints;

    // Start is called before the first frame update
    void Start()
    {
        tank2 = GameObject.Find("Tank2");
        TextComponent = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        tank2 = GameObject.Find("Tank2");
        tank2TurnPoints = tank2.GetComponent<Tank>().turnPoints;
        TextComponent.text = "Player 2 Turn Points: " + tank2TurnPoints;
    }
}
