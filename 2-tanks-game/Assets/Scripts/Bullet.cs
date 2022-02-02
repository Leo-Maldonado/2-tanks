using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    // Radius of the bullet's explosion
    public int explosionRadius = 3;

    // Destroy the terrain wherever the bullet collides with the tilemap
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>())
        {
            FindObjectOfType<Tilemap>().GetComponent<TerrainDestroyer>().DestroyTerrain(this.transform.position, explosionRadius);
            Destroy(this.gameObject);
        }
    }
}
