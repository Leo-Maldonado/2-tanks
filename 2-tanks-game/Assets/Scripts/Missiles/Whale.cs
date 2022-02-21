using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : MonoBehaviour
{
    // Fish that the whale can spawn
    public GameObject fish;

    private void Update()
    {
        // Spawn a fish missile whenever the player clicks
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Spawn fish slightly below the whale
            Vector3 pos = transform.position;
            pos.y -= 1;
            Instantiate(fish, pos, Quaternion.identity);
        }
    }
}
