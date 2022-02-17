using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject tank1;

    private GameObject tank2;

    private TMP_Text TextComponent;

    public bool GameOver = false;

    private bool Running = false;


    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        tank2 = GameObject.Find("Tank2");
        TextComponent = GetComponent<TMP_Text>();
    }

    IEnumerator Wait()
    {
        Running = true;
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    // Update is called once per frame
    void Update()
    {
        int t1health;
        tank1 = GameObject.Find("Tank1");
        t1health = tank1.GetComponent<Tank>().Health;

        int t2health;
        tank2 = GameObject.Find("Tank2");
        t2health = tank2.GetComponent<Tank>().Health;

        if(t1health <= 0)
        {
            TextComponent.text = "Tank 2 Wins!";
            GameOver = true;
        }

        if (t2health <= 0)
        {
            TextComponent.text = "Tank 1 Wins!";
            GameOver = true;
        }

        if(GameOver == true & Running == false)
        {
            StartCoroutine(Wait());
        }
    }
}

