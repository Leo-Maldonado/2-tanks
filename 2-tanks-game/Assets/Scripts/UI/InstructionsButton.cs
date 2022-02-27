using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsButton : MonoBehaviour
{

    public GameObject Panel;

    public void OnInstructionButtonClick()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }
}
