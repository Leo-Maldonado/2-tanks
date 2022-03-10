using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnQuitGameButtonClick()
    {
        //Application.Quit();
        // For WebGL build, just return to main menu
        SceneManager.LoadScene("MainMenu");

    }
}
