using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank2 : MonoBehaviour
{
    // Missile
    public GameObject missilePrefab;

    // Missile velocity
    public float missileVelocity;

    // The two tanks
    private GameObject tank1;

    public int Health = 3;

    public SpriteRenderer sprender;

    // Start is called before the first frame update
    void Start()
    {
        tank1 = GameObject.Find("Tank1");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Bullet>())
        {
            this.Health -= 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get tank if map has been redrawn
        tank1 = GameObject.Find("Tank1");
        // Fire if our turn
        if (Input.GetKeyDown(KeyCode.Mouse0) && tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2
            && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            // Spawn missile slightly above tank to avoid colliding with it *** maybe change this later ***
            Vector3 missilePos = new Vector3(transform.localPosition.x,
                                             transform.localPosition.y + 1);
            GameObject missile = Instantiate(missilePrefab, missilePos, Quaternion.identity);
            // Find mouse position relative to tank position
            Vector3 relativeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * relativeMousePos;
            tank1.GetComponent<Tank1>().playerTurn = Tank1.PlayersTurn.Tank1;
        }
        if(this.Health <= 0)
        {
            sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
        }

    }
}
