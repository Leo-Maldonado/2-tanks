using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMenuExitButton : MonoBehaviour
{
    public GameObject Panel;



    public void OnExitButtonClick()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
    }
}
