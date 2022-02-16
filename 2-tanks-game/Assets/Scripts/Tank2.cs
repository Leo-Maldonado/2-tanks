using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank2 : MonoBehaviour
{
    // Boolean to disable shooting (for testing)
    public bool testing = false;

    // Current missile
    public GameObject currentMissile;

    // Missile manager
    private MissileManager missileManager;

    // Missile velocity
    public float missileVelocity;

    // Object that marks where the player last shot
    public GameObject ShotMarker;

    // Object holding the last shot marker
    private GameObject lastShotMarker;

    // Position of the last shot
    private Vector3 lastShot = new Vector3(0, 0, -100);

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

    // Our turn points
    public float turnPoints = 0;

    // If player has earned turn points this turn
    private bool hasEarnedPoints = false;

    // Apply the specified damage to the tank's health
    public void TakeDamage(int damage)
    {
        int newHealth = this.Health - damage;
        if (newHealth <= 0)
        {
            this.Health = 0;
        }
        else
        {
            this.Health = newHealth;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tank1 = GameObject.Find("Tank1");
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        rigidBody = GetComponent<Rigidbody2D>();
        missileManager = FindObjectOfType<MissileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get tank if map has been redrawn
        tank1 = GameObject.Find("Tank1");

        // Correct turn points
        ManageTurnPoints();

        // Choose missile
        ChooseMissile();
        
        // Display last shot position
        DisplayLastShotPos();

        // Fire if our turn
        Shoot();

        // If we lost
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
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, 0.5f)); //Y value is height / 2
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

    // Flip tank to face direction of motion
    void Flip()
    {
        // Flip so we always face forward
        if (xInput == 1 && facingDirection == -1)
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            transform.position = transform.position + new Vector3(0.5f, 0);
        }
        else if (xInput == -1 && facingDirection == 1)
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            transform.position = transform.position + new Vector3(-0.5f, 0);
        }
    }

    // Internal purchase missile
    private void AttemptPurchaseMissile(GameObject missile)
    {
        bool purchased = missileManager.MissileRequest(turnPoints, missile);
        if (purchased)
        {
            currentMissile = missile;
            turnPoints -= missileManager.missiles[missile];
        }
        else
        {
            Debug.Log("You are broke");
        }
    }

    // Manage turn points
    private void ManageTurnPoints()
    {
        // Get points if haven't already
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2 && !hasEarnedPoints)
        {
            turnPoints += 100;
            hasEarnedPoints = true;
        }
        // If its the other players turn, reset so we can earn next turn
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank1)
        {
            hasEarnedPoints = false;
            currentMissile = missileManager.missile1;
        }
    }

    // Choose missile
    private void ChooseMissile()
    {
        if (tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2 && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AttemptPurchaseMissile(missileManager.missile1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AttemptPurchaseMissile(missileManager.missile2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AttemptPurchaseMissile(missileManager.missile3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AttemptPurchaseMissile(missileManager.missile4);
            }
        }
    }

    // Display last shot position
    private void DisplayLastShotPos()
    {
        if (GameObject.FindGameObjectWithTag("Projectile") == null
            && tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2
            && !gameOverScreen.GameOver
            && lastShot.z > 0
            && !lastShotMarker
            && !testing)
        {
            lastShotMarker = Instantiate(ShotMarker, lastShot, Quaternion.identity);
        }
    }

    // Shoot missiles
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)
            && tank1.GetComponent<Tank1>().playerTurn == Tank1.PlayersTurn.Tank2
            && GameObject.FindGameObjectWithTag("Projectile") == null
            && !gameOverScreen.GameOver
            && !testing)
        {
            // Get rid of the aiming arrow and last shot marker
            Destroy(FindObjectOfType<Arrow>().gameObject);
            Destroy(lastShotMarker);

            // Update last shot position and make sure z coordinate is 1
            lastShot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastShot.z = 1;

            // Find mouse position relative to tank position
            Vector3 relativeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            // Spawn missile in the direction of the arrow (which is also the direction of the mouse)
            Vector3 missilePos = transform.position + relativeMousePos.normalized * 2;
            GameObject missile = Instantiate(currentMissile, missilePos, Quaternion.identity);

            // Add velocity to the missile
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * relativeMousePos;
            tank1.GetComponent<Tank1>().playerTurn = Tank1.PlayersTurn.Tank1;

        }
    }
}
