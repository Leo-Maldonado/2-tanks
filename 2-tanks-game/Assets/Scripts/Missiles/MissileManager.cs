using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    // Dictionary that maps missile object --> missile cost
    public Dictionary<GameObject, float> missiles = new Dictionary<GameObject, float>();

    // Dictionary that maps missile name --> missile object
    public Dictionary<string, GameObject> missileObjects = new Dictionary<string, GameObject>();

    // Struct to hold missile name, game object, and cost
    // -> enables there to be one single place to add new missiles into the UI/game
    [System.Serializable]
    public struct missileInfo
    {
        public string name;
        public GameObject missile;
        public float cost;
    }

    // Array to fill with missiles in the inspector
    // This is the only place a new missile needs to be added!
    public missileInfo[] missileArray;

    // Start
    private void Start()
    {
        // Loop through and add all missiles to the relevant dictionaries
        foreach (missileInfo missileInfo in missileArray)
        {
            missiles.Add(missileInfo.missile, missileInfo.cost);
            missileObjects.Add(missileInfo.name, missileInfo.missile);
        }
    }

    // Request a missile - returns true if purchased
    public bool MissileRequest(float points, string missile)
    {
        // If we can afford the missile
        if (missiles[missileObjects[missile]] <= points)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
