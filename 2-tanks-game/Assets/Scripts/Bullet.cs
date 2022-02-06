using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    // Radius of the bullet's explosion
    public int explosionRadius = 3;
    public bool IsInvisible = false;

    private GameObject tank1;
    private GameObject tank2;

    private void Start()
    {
        this.tank1 = GameObject.Find("Tank1");
        this.tank2 = GameObject.Find("Tank2");
    }


    private void Update()
    {
        if(IsInvisible == true)
        {
            if (transform.position.x < -16f || transform.position.x > 37f || transform.position.y < 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Destroy the terrain wherever the bullet collides with the tilemap
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>())
        {
            FindObjectOfType<Tilemap>().GetComponent<TerrainDestroyer>().DestroyTerrain(this.transform.position, explosionRadius);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.GetComponent<Tank1>())
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.GetComponent<Tank2>())
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