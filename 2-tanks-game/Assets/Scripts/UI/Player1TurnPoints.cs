using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player1TurnPoints : MonoBehaviour
{
    // Tank
    private GameObject tank1;

    // Text
    private TMP_Text TextComponent;

    // Tank's turn points
    private float tank1TurnPoints;

    // Start is called before the first frame update
    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        TextComponent = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        tank1 = GameObject.Find("Tank1");
        tank1TurnPoints = tank1.GetComponent<Tank>().turnPoints;
        TextComponent.text = tank1TurnPoints.ToString();
    }
}
