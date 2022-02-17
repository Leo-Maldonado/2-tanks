using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Tank number that this health bar belongs to (1 for tank1, 2 for tank2)
    public int tankNum;

    // Actual tank that the health bar belongs to
    private GameObject tank;

    // Color that fills the health bar
    public Image fill;

    // Slider to adjust the health displayed
    private Slider slider;

    // Gradient to enable color changes with health loss
    public Gradient gradient;

    // Rect transform to move health bar
    private RectTransform rectTransform;

    // Get slider from this health bar
    private void Start()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Find the appropriate tank
        // -> this doesn't work in the Start() method, probably because of the delayed spawn of tanks to find an appropriate position
        if (tankNum == 1)
        {
            tank = GameObject.Find("Tank1");
        }
        else
        {
            tank = GameObject.Find("Tank2");
        }
        // Update health
        slider.value = tank.GetComponent<Tank>().Health;
        // Update color of health bar
        fill.color = gradient.Evaluate(slider.normalizedValue);
        // Update position to be above the tank
        Vector3 tankPos = tank.transform.position;
        tankPos.y += 1;
        rectTransform.position = Camera.main.WorldToScreenPoint(tankPos);
    }
}
