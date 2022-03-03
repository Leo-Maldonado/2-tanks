using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGameButton : MonoBehaviour
{
    public GameObject Panel;

    public void OnResumeButtonClick()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
