using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank1 : MonoBehaviour
{
    // Boolean to disable shooting (for testing)
    public bool testing = false;

    // Which tanks turn it is to shoot
    public PlayersTurn playerTurn = PlayersTurn.Tank1;
    public enum PlayersTurn { Tank1, Tank2 }

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

    // Health
    public int Health = 100;

    // Sprite Renderer
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
    private float facingDirection = 1;

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

    // Start
    void Start()
    {
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        rigidBody = GetComponent<Rigidbody2D>();
        missileManager = FindObjectOfType<MissileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Correct turn points
        ManageTurnPoints();

        // Choose missile
        ChooseMissile();

        // Display last shot position
        DisplayLastShotPos();

        // Shoot
        Shoot();

        // If we lost or want to quit
        if (this.Health <= 0)
        {
            sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Input
        xInput = Input.GetAxisRaw("Horizontal");

    }

    // FixedUpdate for movement
    void FixedUpdate()
    {
        if (playerTurn == PlayersTurn.Tank1 && GameObject.FindGameObjectWithTag("Projectile") == null)
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
        if (playerTurn == PlayersTurn.Tank1 && !hasEarnedPoints)
        {
            turnPoints += 100;
            hasEarnedPoints = true;
        }
        // If its the other players turn, reset so we can earn next turn
        if (playerTurn == PlayersTurn.Tank2)
        {
            hasEarnedPoints = false;
            currentMissile = missileManager.missile1;
        }
    }

    // Choose missile
    private void ChooseMissile()
    {
        if (playerTurn == PlayersTurn.Tank1 && GameObject.FindGameObjectWithTag("Projectile") == null)
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
            && playerTurn == PlayersTurn.Tank1
            && !gameOverScreen.GameOver
            && lastShot.z > 0
            && !lastShotMarker
            && !testing)
        {
            lastShotMarker = Instantiate(ShotMarker, lastShot, Quaternion.identity);
        }
    }

    // Shoot your gun
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)
            && GameObject.FindGameObjectWithTag("Projectile") == null
            && playerTurn == PlayersTurn.Tank1
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


            //calculate rotation to spawn with
            var dir = -1 * rigidBody.velocity;
            var angle = Mathf.Atan2(relativeMousePos.y, relativeMousePos.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            // Spawn missile in the direction of the arrow (which is also the direction of the mouse)
            Vector3 normalized = relativeMousePos.normalized;
            Vector3 missilePos = transform.position + normalized * 4f;
            GameObject missile = Instantiate(currentMissile, missilePos, Quaternion.identity);

            //charge player for purchasing missile
            turnPoints -= missileManager.missiles[currentMissile];

            
            //find distance between mouse and tank
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xDist =  mousePos.x - transform.position.x;
            float yDist = mousePos.y - transform.position.y;

            //clamp relativeMousePos to max/min values (got these just from testing in scene they're not perfect, but more than good enough i think)
            float xVelo = Mathf.Clamp(xDist, 0.025f, 16f);
            float yVelo = Mathf.Clamp(yDist, .025f, 16f);

            if (xDist < 0)
            {
                xVelo = Mathf.Clamp(xDist, -16f, -.025f);
            }
            if (yDist < 0)
            {
                yVelo = Mathf.Clamp(yDist, -16f, -.025f);
            }

            //scales to better fit velocity, 4.6f comes from scaling 1f-16f to 1f-3.5f
            //multiply bu 7.5f to make it actually have velocity, came to 7.5f through testing
            xVelo /= 4.6f;
            yVelo /= 4.6f;
            xVelo *= 7.5f;
            yVelo *= 7.5f;

            Vector3 vector = new Vector3(xVelo, yVelo, 0.0f);
            // Add velocity to the missile
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * vector;
            playerTurn = PlayersTurn.Tank2;
        }
    }
}