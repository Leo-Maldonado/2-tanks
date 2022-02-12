using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Missile : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject tank2;

    public GameObject ChoiceUI;

    public GameObject UI1;
    // Start is called before the first frame update

    public Tank2.BulletType bulletType;
    void Start()
    {
        this.tank2 = GameObject.Find("Tank2");
        Vector3 spawnPos = new Vector3(34.5f, 20.5f, 0f);
        UI1 = Instantiate(ChoiceUI, spawnPos, Quaternion.identity);
        this.bulletType = Tank2.BulletType.Bullet1;
        UI1.GetComponent<MissileChoiceUI>().Player2Change(Tank2.BulletType.Bullet1);
    }

    // Update is called once per frame
    void Update()
    {
        this.tank2 = GameObject.Find("Tank2");
        if (this.tank2.GetComponent<Tank2>().bullet != this.bulletType)
        {
            this.bulletType = this.tank2.GetComponent<Tank2>().bullet;
            UI1.GetComponent<MissileChoiceUI>().Player2Change(this.bulletType);
        }
    }
}
