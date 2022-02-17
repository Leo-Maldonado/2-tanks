using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player1Health : MonoBehaviour
{
    // Start is called before the first frame update


    private GameObject tank1;

    private TMP_Text TextComponent;

    private int t1health = 100;

    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        TextComponent = GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        tank1 = GameObject.Find("Tank1");
        t1health = tank1.GetComponent<Tank>().Health;
        TextComponent.text = "Player 1 Health: " + t1health;
    }
}
