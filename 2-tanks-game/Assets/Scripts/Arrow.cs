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

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Scale with distance + move with tank
        GameObject tank1 = GameObject.Find("Tank1");
        GameObject tank2 = GameObject.Find("Tank2");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank1 && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            //Scale
            float dist = Vector3.Distance(tank1.transform.position, mousePos);
            float xScale = Mathf.Min(maxXScale, Mathf.Max(minXScale, Mathf.Pow(dist / 10, 2)));
            float yScale = Mathf.Min(maxYScale, Mathf.Max(minYScale, Mathf.Pow(dist / 10, 2)));
            transform.localScale = new Vector3(xScale, yScale);
            //Follow tank
            Vector3 moveTo = new Vector3(tank1.transform.position.x + 1.3f, tank1.transform.position.y + 0.3f);
            transform.position = Vector3.MoveTowards(transform.position, moveTo, .1f);
        }
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2 && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            //Scale
            float dist = Vector3.Distance(tank2.transform.position, mousePos);
            float xScale = Mathf.Min(maxXScale, Mathf.Max(minXScale, Mathf.Pow(dist / 10, 2)));
            float yScale = Mathf.Min(maxYScale, Mathf.Max(minYScale, Mathf.Pow(dist / 10, 2)));
            transform.localScale = new Vector3(xScale, yScale);
            //Follow tank
            Vector3 moveTo = new Vector3(tank2.transform.position.x - 1.3f, tank2.transform.position.y + 0.3f);
            transform.position = Vector3.MoveTowards(transform.position, moveTo, .1f);
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
