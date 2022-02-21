using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    // Right now this is just going to hold different missile types for the tanks to access them
    public GameObject missile1;

    public float missile1Cost;

    public GameObject missile2;

    public float missile2Cost;

    public GameObject missile3;

    public float missile3Cost;

    public GameObject missile4;

    public float missile4Cost;

    public GameObject missile5;

    public float missile5Cost;

    // Dictionary with missiles and costs
    public Dictionary<GameObject, float> missiles = new Dictionary<GameObject, float>();

    // Start
    private void Start()
    {
        missiles.Add(missile1, missile1Cost);
        missiles.Add(missile2, missile2Cost);
        missiles.Add(missile3, missile3Cost);
        missiles.Add(missile4, missile4Cost);
        missiles.Add(missile5, missile5Cost);
    }

    // Request a missile - returns true if purchased
    public bool MissileRequest(float points, GameObject missile)
    {
        // If we can afford the missile
        if (missiles[missile] <= points)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
