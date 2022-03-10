using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject panel;

    public bool IsPaused = false;

    public void OnPause()
    {
        if (panel)
        {
            Time.timeScale = 0;
            panel.SetActive(true);
            IsPaused = true;
        }
    }
}
