using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank2 : MonoBehaviour
{
    // Missile
    public GameObject missilePrefab;

    // Missile velocity
    public float missileVelocity;

    // The two tanks
    private GameObject tank1;

    // Start is called before the first frame update
    void Start()
    {
        tank1 = GameObject.Find("Tank1");
    }

    // Update is called once per frame
    void Update()
    {
        // Get tank if map has been redrawn
        tank1 = GameObject.Find("Tank1");
        // Fire if our turn
        if (Input.GetKeyDown(KeyCode.Mouse0) && tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2)
        {
            Vector3 missilePos = new Vector3(transform.localPosition.x + transform.right.x,
                                             transform.localPosition.y + transform.right.y);
            GameObject missile = Instantiate(missilePrefab, missilePos, Quaternion.identity);
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tank1.GetComponent<Tank1>().playerTurn = Tank1.PlayersTurn.Tank1;
        }
    }
}
