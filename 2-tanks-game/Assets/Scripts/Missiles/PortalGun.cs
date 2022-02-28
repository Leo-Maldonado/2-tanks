using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    // Portal that moves the player whose turn it is
    public GameObject portal;

    // Spawn the portal upon collision and let it handle everything else
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 pos = transform.position;
        // Portal spawns 5 above the collision
        pos.y += 5;
        Instantiate(portal, pos, Quaternion.identity);
    }
}
