using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Missile : MonoBehaviour
{
    public GameObject tank2;

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
        if(tank2 == null)
        {
            tank2 = GameObject.Find("Tank2");
            Vector3 spawnPos = new Vector3(30.6f, 23.125f, 0f);
            UI1 = Instantiate(ChoiceUI, spawnPos, Quaternion.identity);
            displayedMissile = tank2.GetComponent<Tank>().currentMissile;
            UI1.GetComponent<MissileChoiceUI>().Player2Change(displayedMissile);
        }
      
        //tank2 = GameObject.Find("Tank2");
        if (this.tank2.GetComponent<Tank>().currentMissile != displayedMissile)
        {
            displayedMissile = this.tank2.GetComponent<Tank>().currentMissile;
            UI1.GetComponent<MissileChoiceUI>().Player2Change(displayedMissile);
        }
    }
}
