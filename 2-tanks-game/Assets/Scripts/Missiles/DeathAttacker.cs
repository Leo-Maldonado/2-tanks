using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeathAttacker : MonoBehaviour
{
    // Casting death bringer
    public GameObject caster;

    // Position to spawn the spell casting death bringer
    private Vector3 aboveCollision;

    // Damage of the death bringer's attack
    private int damage = 50;

    // Explosion radius of the death bringer's attack
    private int explosionRadius = 6;

    void Start()
    {
        aboveCollision = transform.position;
        aboveCollision.y = 21;
        // Deal damage and destroy terrain
        Invoke("DestroyStuff", 1f);
        // Summon caster and destroy attacker 
        Invoke("SummonCaster", 2.3f);
        Invoke("DestroyAttacker", 2.3f);
    }

    // Destroy terrain and damage tanks when the attacker swings his weapon
    private void DestroyStuff()
    {
        GameObject tank1 = GameObject.Find("Tank1");
        GameObject tank2 = GameObject.Find("Tank2");
        // Destroy nearby terrain
        FindObjectOfType<Tilemap>().GetComponent<TerrainDestroyer>().DestroyTerrain(this.transform.position, explosionRadius);
        // Deal damage to any tanks within the explosion radius
        // damage is decreased based on how far the projectile explodes from the tank
        float tank1Distance = Vector3.Distance(transform.position, tank1.transform.position);
        float tank2Distance = Vector3.Distance(transform.position, tank2.transform.position);
        if (tank1Distance < explosionRadius)
        {
            float damageScale = (explosionRadius - tank1Distance) / explosionRadius;
            tank1.GetComponent<Tank>().TakeDamage(Mathf.RoundToInt(damage * damageScale));
        }
        if (tank2Distance < explosionRadius)
        {
            float damageScale = (explosionRadius - tank2Distance) / explosionRadius;
            tank2.GetComponent<Tank>().TakeDamage(Mathf.RoundToInt(damage * damageScale));
        }
    }

    // Summon the spell casting death bringer
    private void SummonCaster()
    {
        Instantiate(caster, aboveCollision, Quaternion.identity);
    }

    // Destroy the attacking death bringer
    private void DestroyAttacker()
    {
        GameObject attacker = GameObject.Find("Attack(Clone)");
        Destroy(attacker);

    }
}
