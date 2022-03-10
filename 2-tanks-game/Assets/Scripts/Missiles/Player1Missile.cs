using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Missile : MonoBehaviour
{

    public GameObject tank1;

    public GameObject ChoiceUI;

    public GameObject UI1;

    private Transform t1Img;

    // Missile that is being displayed
    private GameObject displayedMissile;
    
    void Start()
    {
        t1Img = GameObject.Find("Player1HUD").transform.Find("TankImage");
    }


    // Update is called once per frame
    void Update()
    {
        if(tank1 == null)
        {
            tank1 = GameObject.Find("Tank1");
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(t1Img.position) + new Vector3(2.6f, 0.325f, 10f);
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
