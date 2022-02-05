using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank1 : MonoBehaviour
{
    // Which tanks turn it is to shoot
    public PlayersTurn playerTurn = PlayersTurn.Tank1;
    public enum PlayersTurn { Tank1, Tank2 }

    // Missile
    public GameObject missilePrefab;

    // Missile velocity
    public float missileVelocity;

    public int Health = 3;

    public SpriteRenderer sprender;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            this.Health -= 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerTurn == PlayersTurn.Tank1
            && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            // Spawn missile slightly above tank to avoid colliding with it *** maybe change this later ***
            Vector3 missilePos = new Vector3(transform.localPosition.x,
                                             transform.localPosition.y + 1);
            GameObject missile = Instantiate(missilePrefab, missilePos, Quaternion.identity);
            // Find mouse position relative to tank position
            Vector3 relativeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * relativeMousePos;
            playerTurn = PlayersTurn.Tank2;
        }
        if(this.Health <= 0)
        {
            sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
        }

    }
}
