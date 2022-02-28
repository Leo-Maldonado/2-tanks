using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Find the appropriate tank to move, move it, and then destroy the portal
    void Start()
    {
        Invoke("MoveTank", .5f);
        Invoke("Destroy", 1f);
    }

    void MoveTank()
    {
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        GameObject tank;
        if (turnManager.IsPlayerTurn(1))
        {
            tank = GameObject.Find("Tank2");
        }
        else
        {
            tank = GameObject.Find("Tank1");
        }
        tank.transform.position = transform.position;
    }

    // Function to destroy the portal
    void Destroy()
    {
        Destroy(gameObject);
    }
}
