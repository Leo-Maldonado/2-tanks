using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuObject : MonoBehaviour
{
    
    public GameObject Menu;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //stops projectile in the air but also leads to some weird bugs
        //need to make sure the player can't shoot when the pause menu is open
        if (Input.GetKeyDown(KeyCode.Escape) & !Menu.activeSelf)
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) & Menu.activeSelf)
        {
            Menu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
