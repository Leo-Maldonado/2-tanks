using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Rigidbody
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Scale with distance
        GameObject tank1 = GameObject.Find("Tank1");
        GameObject tank2 = GameObject.Find("Tank2");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank1 && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            float dist = Vector3.Distance(tank1.transform.position, mousePos);
            float xScale = Mathf.Max(1f, dist / 20);
            float yScale = Mathf.Max(0.25f, dist / 15);
            transform.localScale = new Vector3(xScale, yScale);
        }
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2 && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            float dist = Vector3.Distance(tank2.transform.position, mousePos);
            float xScale = Mathf.Max(1f, dist / 20);
            float yScale = Mathf.Max(0.25f, dist / 15);
            transform.localScale = new Vector3(xScale, yScale);
        }
    }

    private void FixedUpdate()
    {
        // Aim to face mouse
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(Input.mousePosition.y - screenPos.y, Input.mousePosition.x - screenPos.x) * Mathf.Rad2Deg;
        rigidBody.MoveRotation(angle - 90);
    }
}
