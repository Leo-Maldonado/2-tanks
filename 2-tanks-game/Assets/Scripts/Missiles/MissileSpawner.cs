using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    // Up to 5 missile prefabs to test
    public GameObject M1;

    public GameObject M2;

    public GameObject M3;

    public GameObject M4;

    public GameObject M5;

    // Current missile
    private GameObject missile;

    // Start as missile 1
    // Also, disable tanks being able to fire and hide any arrows
    private void Start()
    {
        missile = M1;
        GameObject.FindObjectOfType<Tank1>().testing = true;
        GameObject.FindObjectOfType<Tank2>().testing = true;
        foreach(Arrow arrow in GameObject.FindObjectsOfType<Arrow>())
        {
            arrow.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn a missile wherever we click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPos.z = 1;
            Instantiate(missile, spawnPos, Quaternion.identity);
        }

        // Switch missiles by pressing the number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.missile = M1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.missile = M2;

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.missile = M3;

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.missile = M4;

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            this.missile = M5;

        }
    }
}
