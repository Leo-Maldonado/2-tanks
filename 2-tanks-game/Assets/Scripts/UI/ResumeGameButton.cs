using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGameButton : MonoBehaviour
{
    public GameObject Panel;

    //private bool isPaused;

    void Start()
    {
        //isPaused = GameObject.Find("PauseButton").GetComponent<PauseButton>().IsPaused;
    }

    public void OnResumeButtonClick()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
            Time.timeScale = 1;
            GameObject.Find("PauseButton").GetComponent<PauseButton>().IsPaused = false;
        }
    }
}
