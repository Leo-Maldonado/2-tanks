using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject panel;

    public void OnPause()
    {
        if (panel)
        {
            Time.timeScale = 0;
            panel.SetActive(true);
        }
    }
}
