using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAngryBird : MonoBehaviour
{
    // Blue birds that are spawned when the player clicks
    public GameObject bird;

    // Split into three birds when the player clicks
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 velocity = GetComponent<Rigidbody2D>().velocity;
            Vector3 pos = transform.position;

            // First bird has the same trajectory as the original projectile
            GameObject bird1 = Instantiate(bird, pos, Quaternion.identity);
            bird1.GetComponent<Rigidbody2D>().velocity = velocity;

            // Second bird has slightly higher y component of velocity
            GameObject bird2 = Instantiate(bird, pos, Quaternion.identity);
            Vector3 bird2Velocity = velocity;
            bird2Velocity.y += 2;
            bird2.GetComponent<Rigidbody2D>().velocity = bird2Velocity;

            // Third bird has slightly lower y component of velocity
            GameObject bird3 = Instantiate(bird, pos, Quaternion.identity);
            Vector3 bird3Velocity = velocity;
            bird3Velocity.y -= 2;
            bird3.GetComponent<Rigidbody2D>().velocity = bird3Velocity;

            Destroy(gameObject);
        }
    }
}
