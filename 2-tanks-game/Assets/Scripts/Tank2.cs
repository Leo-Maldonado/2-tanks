using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank2 : MonoBehaviour
{
    // Missile
    public GameObject missilePrefab;

    // Missile velocity
    public float missileVelocity;

    // The two tanks
    private GameObject tank1;

    // Health
    public int Health = 100;

    // Sprite renderer
    public SpriteRenderer sprender;

    // GameOverScreen instance to access is game running
    private GameOverScreen gameOverScreen;

    // Our rigidbody
    private Rigidbody2D rigidBody;

    // Speed for movement
    public float movementSpeed;

    // Our polygon collider
    private CapsuleCollider2D capCollider;

    // If we are on a slope
    private bool isOnSlope;

    // Direction we are facing
    private float facingDirection = -1;

    // The x input
    private float xInput;

    // Slope check distance
    [SerializeField]
    private float slopeCheckDistance;

    // Layermask holding ground layer
    [SerializeField]
    private LayerMask whatIsGround;

    // Apply the specified damage to the tank's health
    public void TakeDamage(int damage)
    {
        this.Health -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        rigidBody = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get tank if map has been redrawn
        tank1 = GameObject.Find("Tank1");
        // Fire if our turn
        if (Input.GetKeyDown(KeyCode.Mouse0)
            && tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2
            && GameObject.FindGameObjectWithTag("Projectile") == null
            && !gameOverScreen.GameOver
            )
        {
            // Spawn missile with arrow
            Vector3 missilePos = new Vector3(transform.localPosition.x - 1.5f,
                                             transform.localPosition.y + 1.5f);
            GameObject missile = Instantiate(missilePrefab, missilePos, Quaternion.identity);
            // Find mouse position relative to tank position
            Vector3 relativeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * relativeMousePos;
            tank1.GetComponent<Tank1>().playerTurn = Tank1.PlayersTurn.Tank1;
        }
        if(this.Health <= 0)
        {
            sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
        }
        // Input
        xInput = Input.GetAxisRaw("Horizontal");

    }

    // FixedUpdate for movement
    void FixedUpdate()
    {
        // Get tank if map has been redrawn
        tank1 = GameObject.Find("Tank1");
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2 && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            Flip();
            SlopeCheck();
            ApplyMovement();
        }
    }

    // Check if we are on a slope
    void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capCollider.size.y / 2));
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);
        if (slopeHitFront || slopeHitBack)
        {
            isOnSlope = true;
        }
        else
        {
            isOnSlope = false;
        }
    }

    // Movement based on if we are on a slope or not
    void ApplyMovement()
    {
        if (!isOnSlope)
        {
            rigidBody.velocity = new Vector2(movementSpeed * xInput, 0.0f);
        }
        else
        {
            if (xInput == 1)
            {
                rigidBody.velocity = new Vector2(movementSpeed * -0.8f * -xInput, movementSpeed * -0.8f * -xInput);
            }
            else
            {
                rigidBody.velocity = new Vector2(movementSpeed * -0.8f * -xInput, movementSpeed * 0.8f * -xInput);
            }
        }
    }

    void Flip()
    {
        // Flip so we always face forward
        if (xInput == 1 && facingDirection == -1)
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if (xInput == -1 && facingDirection == 1)
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
}
