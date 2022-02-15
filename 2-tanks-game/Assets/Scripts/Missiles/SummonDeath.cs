using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDeath : MonoBehaviour
{
    // Attacker
    public GameObject attack;

    // Spawn the attacker and let that game object do the rest
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y += 2;
        Instantiate(attack, spawnPos, Quaternion.identity);
    }
}
