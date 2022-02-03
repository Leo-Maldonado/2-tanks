using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    // Fish prefab
    public GameObject fish;

    // Update is called once per frame
    void Update()
    {
        // Spawn a fish wherever the player clicks (for fun)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPos.z = 1;
            //SpawnFishAtLocation(spawnPos);
        }
    }

    // Spawn a fish at specified location
    void SpawnFishAtLocation(Vector3 spawnPos)
    {
        Instantiate(fish, spawnPos, Quaternion.identity);
    }
}
