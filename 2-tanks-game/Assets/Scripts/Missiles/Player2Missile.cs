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
        tank2 = GameObject.Find("Tank2");
        Vector3 spawnPos = new Vector3(34.5f, 20.5f, 0f);
        UI1 = Instantiate(ChoiceUI, spawnPos, Quaternion.identity);
        displayedMissile = tank2.GetComponent<Tank>().currentMissile;
        UI1.GetComponent<MissileChoiceUI>().Player2Change(displayedMissile);
    }

    // Update is called once per frame
    void Update()
    {
        tank2 = GameObject.Find("Tank2");
        if (this.tank2.GetComponent<Tank>().currentMissile != displayedMissile)
        {
            displayedMissile = this.tank2.GetComponent<Tank>().currentMissile;
            UI1.GetComponent<MissileChoiceUI>().Player2Change(displayedMissile);
        }
    }
}
