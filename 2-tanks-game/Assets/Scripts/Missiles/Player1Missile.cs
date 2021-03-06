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
       
    }


    // Update is called once per frame
    void Update()
    {
        if(tank1 == null) {
            tank1 = GameObject.Find("Tank1");
            Vector3 spawnPos = new Vector3(-10.3f, 23.125f, 0f);
            Quaternion spawnRot = new Quaternion(0, 180, 0, 0);
            UI1 = Instantiate(ChoiceUI, spawnPos, spawnRot);
            displayedMissile = tank1.GetComponent<Tank>().currentMissile;
            UI1.GetComponent<MissileChoiceUI>().Player1Change(displayedMissile);
        }
    

        //tank1 = GameObject.Find("Tank1");
        if (tank1.GetComponent<Tank>().currentMissile != displayedMissile)
        {
            displayedMissile = tank1.GetComponent<Tank>().currentMissile;
            UI1.GetComponent<MissileChoiceUI>().Player1Change(displayedMissile);
        }
    }
}
