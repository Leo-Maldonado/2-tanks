using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileChoiceUI : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private MissileManager missileManager;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        missileManager = FindObjectOfType<MissileManager>();
    }

    public void Player1Change(GameObject missile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = missile.GetComponent<SpriteRenderer>().sprite;
        // make sure sprite is scaled correctly
        transform.localScale = missile.transform.localScale;
    }

    public void Player2Change(GameObject missile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = missile.GetComponent<SpriteRenderer>().sprite;
        // make sure sprite is scaled correctly
        transform.localScale = missile.transform.localScale;
    }

}
