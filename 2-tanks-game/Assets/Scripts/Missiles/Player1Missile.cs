using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Missile : MonoBehaviour
{

    public GameObject tank1;

    public GameObject ChoiceUI;

    public GameObject UI1;

    // Missile that is being displayed
    private GameObject displayedMissile;
    
    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        Vector3 spawnPos = new Vector3(-6f, 20.5f, 0f);
        UI1 = Instantiate(ChoiceUI, spawnPos, Quaternion.identity);
        displayedMissile = tank1.GetComponent<Tank>().currentMissile;
        UI1.GetComponent<MissileChoiceUI>().Player1Change(displayedMissile);
    }

    // Update is called once per frame
    void Update()
    {
        tank1 = GameObject.Find("Tank1");
        if (tank1.GetComponent<Tank>().currentMissile != displayedMissile)
        {
            displayedMissile = tank1.GetComponent<Tank>().currentMissile;
            UI1.GetComponent<MissileChoiceUI>().Player1Change(displayedMissile);
        }
    }
}
