using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCaster : MonoBehaviour
{
    // Missile the spell will spawn
    public GameObject spellMissile;

    void Start()
    {
        Invoke("SpawnMissiles", 1f);
        Invoke("SpawnMissiles", 2f);
        Invoke("DestroyCaster", 3f);
    }

    // Spawn missiles for the spell
    private void SpawnMissiles()
    {
        for (float i = -3f; i <= 3; i+= 1.5f)
        {
            Vector3 newPos = transform.position;
            newPos.x += i;
            Instantiate(spellMissile, newPos, Quaternion.identity);
        }
    }

    // Destroy the spell casting bringer of death
    private void DestroyCaster()
    {
        GameObject caster = GameObject.Find("Cast Spell(Clone)");
        Destroy(caster);
    }
}
