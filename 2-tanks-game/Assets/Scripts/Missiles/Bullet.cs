using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    // Radius of the bullet's explosion
    public int explosionRadius = 3;
    // How much damage the projectile does when it directly hits a tank
    public int damage = 30;

    public bool IsInvisible = false;

    private GameObject tank1;
    private GameObject tank2;

    public Rigidbody2D rigid;

    private void Start()
    {
        this.tank1 = GameObject.Find("Tank1");
        this.tank2 = GameObject.Find("Tank2");
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        var dir = -1 * rigid.velocity;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if(IsInvisible == true)
        {
            if (transform.position.x < -16f || transform.position.x > 37f || transform.position.y < 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Destroy the terrain wherever the bullet collides with the tilemap
    // Also destroy terrain within the explosion radius when a tank is hit directly
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>())
        {
            FindObjectOfType<Tilemap>().GetComponent<TerrainDestroyer>().DestroyTerrain(this.transform.position, explosionRadius);
            // Deal damage to any tanks within the explosion radius
            // damage is decreased based on how far the projectile explodes from the tank
            float tank1Distance = Vector3.Distance(transform.position, tank1.transform.position);
            float tank2Distance = Vector3.Distance(transform.position, tank2.transform.position);
            if (tank1Distance < explosionRadius)
            {
                float damageScale = (explosionRadius - tank1Distance) / explosionRadius;
                tank1.GetComponent<Tank1>().TakeDamage(Mathf.RoundToInt(damage * damageScale));
            }
            if (tank2Distance < explosionRadius)
            {
                float damageScale = (explosionRadius - tank2Distance) / explosionRadius;
                tank2.GetComponent<Tank2>().TakeDamage(Mathf.RoundToInt(damage * damageScale));
            }
            Destroy(this.gameObject);
        }
        if (collision.gameObject.GetComponent<Tank1>())
        {
            //this seems counter inteuitive, to damage tank2 only when it is its turn. But the way the tanks change is right after they
            //shoot, so by the time it notices the collision, the player turn has already changed
            if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank1)
            {
                tank1.GetComponent<Tank1>().TakeDamage(damage);
                FindObjectOfType<Tilemap>().GetComponent<TerrainDestroyer>().DestroyTerrain(this.transform.position, explosionRadius);
                Destroy(this.gameObject);
            }
              
   
        }
        if (collision.gameObject.GetComponent<Tank2>())
        {
            //this seems counter inteuitive, to damage tank2 only when it is its turn. But the way the tanks change is right after they
            //shoot, so by the time it notices the collision, the player turn has already changed
            if(tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2)
            {
                tank2.GetComponent<Tank2>().TakeDamage(damage);
                FindObjectOfType<Tilemap>().GetComponent<TerrainDestroyer>().DestroyTerrain(this.transform.position, explosionRadius);
                Destroy(this.gameObject);
            }
        }
        // Destroy the projectile if it collides with a barrier
        if (collision.collider.gameObject.tag == "Barrier")
        {
            Destroy(this.gameObject);
        }
    }

    // Destroy when goes off screen
    private void OnBecameInvisible()
    {
        if (transform.position.x < -16f || transform.position.x > 37f)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.IsInvisible = true;
        }
    }
}