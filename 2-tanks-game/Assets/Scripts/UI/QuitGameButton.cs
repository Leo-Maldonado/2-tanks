using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnQuitGameButtonClick()
    {
        Application.Quit();
    }
}
