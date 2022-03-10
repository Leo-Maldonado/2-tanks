using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Missile : MonoBehaviour
{
    public GameObject tank2;

    public GameObject ChoiceUI;

    public GameObject UI1;

    private Transform t2Img;

    // Missile that is being displayed
    private GameObject displayedMissile;

    void Start()
    {
        t2Img = GameObject.Find("Player2HUD").transform.Find("TankImage");
    }

    // Update is called once per frame
    void Update()
    {
        if(tank2 == null)
        {
            tank2 = GameObject.Find("Tank2");
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(t2Img.position) - new Vector3(2.7f, -0.425f, -10f);
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
