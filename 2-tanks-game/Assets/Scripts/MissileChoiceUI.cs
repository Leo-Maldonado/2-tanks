using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileChoiceUI : MonoBehaviour
{

    public GameObject tank1;

    public SpriteRenderer spriteRenderer;

    public Sprite fish;
    public Sprite missile1;
    public Sprite missile2;

    public Tank1.BulletType bullet;

    public Tank2.BulletType bullet2;

    // Start is called before the first frame update
    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void Player1Change(Tank1.BulletType bullet)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (bullet == Tank1.BulletType.Bullet1)
        {
            spriteRenderer.sprite = fish;
            this.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }
        if (bullet == Tank1.BulletType.Bullet2)
        {
            spriteRenderer.sprite = missile1;
            this.transform.localScale = new Vector3(3f, 3f, 1f);
        }
        if (bullet == Tank1.BulletType.Bullet3)
        {
            spriteRenderer.sprite = missile2;
            this.transform.localScale = new Vector3(3f, 3f, 1f);
        }
    }

    public void Player2Change(Tank2.BulletType bullet)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (bullet == Tank2.BulletType.Bullet1)
        {
            spriteRenderer.sprite = fish;
            this.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }
        if (bullet == Tank2.BulletType.Bullet2)
        {
            spriteRenderer.sprite = missile1;
            this.transform.localScale = new Vector3(3f, 3f, 1f);
        }
        if (bullet == Tank2.BulletType.Bullet3)
        {
            spriteRenderer.sprite = missile2;
            this.transform.localScale = new Vector3(3f, 3f, 1f);
        }
    }

}
