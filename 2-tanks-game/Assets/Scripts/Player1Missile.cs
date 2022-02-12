using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Missile : MonoBehaviour
{

    public GameObject tank1;

    public GameObject ChoiceUI;

    public GameObject UI1;
    // Start is called before the first frame update

    public Tank1.BulletType bulletType;
    void Start()
    {
        this.tank1 = GameObject.Find("Tank1");
        Vector3 spawnPos = new Vector3(-6f, 20.5f, 0f);
        UI1 = Instantiate(ChoiceUI, spawnPos, Quaternion.identity);
        this.bulletType = Tank1.BulletType.Bullet1;
        UI1.GetComponent<MissileChoiceUI>().Player1Change(Tank1.BulletType.Bullet1);
    }

    // Update is called once per frame
    void Update()
    {
        this.tank1 = GameObject.Find("Tank1");
        if (this.tank1.GetComponent<Tank1>().bullet != this.bulletType)
        {
            this.bulletType = this.tank1.GetComponent<Tank1>().bullet;
            UI1.GetComponent<MissileChoiceUI>().Player1Change(this.bulletType);
        }

    }
}
