using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Rigidbody
    private Rigidbody2D rigidBody;

    // Min and max arrow scales
    private float maxXScale = 3.5f;
    private float maxYScale = 3.5f;
    private float minXScale = 1;
    private float minYScale = 1;

    // Turn manager to find out who's turn it is
    private TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        turnManager = GameObject.FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Scale with distance + move with tank
        GameObject tank1 = GameObject.Find("Tank1");
        GameObject tank2 = GameObject.Find("Tank2");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (turnManager.IsPlayerTurn(1) && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            //Scale
            float dist = Vector3.Distance(tank1.transform.position, mousePos);
            float xScale = Mathf.Min(maxXScale, Mathf.Max(minXScale, Mathf.Pow(dist / 10, 2)));
            float yScale = Mathf.Min(maxYScale, Mathf.Max(minYScale, Mathf.Pow(dist / 10, 2)));
            transform.localScale = new Vector3(xScale, yScale);
            //Follow tank
            transform.position = tank1.transform.position;
        }
        if (turnManager.IsPlayerTurn(2) && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            //Scale
            float dist = Vector3.Distance(tank2.transform.position, mousePos);
            float xScale = Mathf.Min(maxXScale, Mathf.Max(minXScale, Mathf.Pow(dist / 10, 2)));
            float yScale = Mathf.Min(maxYScale, Mathf.Max(minYScale, Mathf.Pow(dist / 10, 2)));
            transform.localScale = new Vector3(xScale, yScale);
            //Follow tank
            transform.position = tank2.transform.position;
        }
    }

    private void FixedUpdate()
    {
        // Aim to face mouse
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(Input.mousePosition.y - screenPos.y, Input.mousePosition.x - screenPos.x) * Mathf.Rad2Deg;
        //Restrict to forward and up
        if (turnManager.IsPlayerTurn(1))
        {
            angle = Mathf.Clamp(angle, 0, 90);
        }
        else
        {
            if (angle <= 0)
            {
                angle = 180;
            }
            angle = Mathf.Clamp(angle, 90, 180);
        }
        rigidBody.MoveRotation(angle - 90);
    }
}
