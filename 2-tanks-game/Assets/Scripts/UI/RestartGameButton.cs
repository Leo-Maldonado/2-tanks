using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnRestartButtonClick()
    {

       Time.timeScale = 1;
        GameObject.Find("PauseButton").GetComponent<PauseButton>().IsPaused = false;
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
