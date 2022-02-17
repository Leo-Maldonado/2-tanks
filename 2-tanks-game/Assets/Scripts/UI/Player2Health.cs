using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player2Health : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject tank2;

    private TMP_Text TextComponent;

    private int t2health = 100;

    void Start()
    {
        tank2 = GameObject.Find("Tank2");
        TextComponent = GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        tank2 = GameObject.Find("Tank2");
        t2health = tank2.GetComponent<Tank>().Health;
        TextComponent.text = "Player 2 Health: " + t2health;
    }
}
