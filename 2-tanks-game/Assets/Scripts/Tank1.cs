using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank1 : MonoBehaviour
{
    // Which tanks turn it is to shoot
    public PlayersTurn playerTurn = PlayersTurn.Tank1;
    public enum PlayersTurn { Tank1, Tank2 }

    // Missile
    public GameObject missilePrefab;

    // Missile velocity
    public float missileVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerTurn == PlayersTurn.Tank1)
        {
            Vector3 missilePos = new Vector3(transform.localPosition.x + transform.right.x,
                                             transform.localPosition.y + transform.right.y);
            GameObject missile = Instantiate(missilePrefab, missilePos, Quaternion.identity);
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerTurn = PlayersTurn.Tank2;
        }
    }
}
