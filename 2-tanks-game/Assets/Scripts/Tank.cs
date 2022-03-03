using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Tank : MonoBehaviour
{
    // What number tank this is (used to determine whether or not it is this player's turn)
    public int tankNumber;

    // Boolean to disable shooting (for testing)
    public bool testing = false;

    // Current missile
    public GameObject currentMissile;

    // Current missile name
    public string currentMissileName;

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

    // Whether or not it is this tank's turn
    private bool playerTurn;

    // Turn manager to check who's turn it is
    private TurnManager turnManager;

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
    public float facingDirection = 1;

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

    // The buy menu
    private GameObject buyMenu;

    // If we are allowed to move
    private bool canMove = true;


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
        // Get turn manager
        turnManager = GameObject.FindObjectOfType<TurnManager>();
        // Tank 1 always gets the first turn
        playerTurn = tankNumber == 1;
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        rigidBody = GetComponent<Rigidbody2D>();
        missileManager = FindObjectOfType<MissileManager>();
        // Buy menu
        buyMenu = GameObject.Find("Container");
        // Set current missile
        currentMissile = missileManager.missileObjects["Basic Missile"];
        currentMissileName = "Basic Missile";
    }

    // Update is called once per frame
    void Update()
    {
        // Update whether or not it is this tank's turn
        playerTurn = turnManager.GetComponent<TurnManager>().IsPlayerTurn(tankNumber);

        // Correct turn points
        ManageTurnPoints();

        // Choose missile with number keys - deprecated with buy menu
        // ChooseMissile();

        // Shoot
        Shoot();

        // If we lost or want to quit
        CheckLost();
        
        // Input
        xInput = Input.GetAxisRaw("Horizontal");

    }

    // FixedUpdate for movement
    void FixedUpdate()
    {
        // Check if buy menu is open and if we can move
        buyMenu = GameObject.Find("Container");
        
        if (buyMenu)
        {
            this.canMove = false;
        }
        else
        {
            this.canMove = true;
        }

        // Move if allowed
        if (playerTurn && GameObject.FindGameObjectWithTag("Projectile") == null && canMove)
        {
            Flip();
            SlopeCheck();
            ApplyMovement();
        }
    }

    // Check if we lost
    void CheckLost()
    {
        if (this.Health <= 0)
        {
            // Disable tank sprite
            sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
        }
        // Disable all movement
        if (gameOverScreen.GameOver)
        {
            canMove = false;
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
        // Move
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
        if (turnManager.IsPlayerTurn(1))
        {
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
        else
        {
            if (xInput == 1 && facingDirection == 1)
            {
                facingDirection *= -1;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                transform.position = transform.position + new Vector3(0.5f, 0);
            }
            else if (xInput == -1 && facingDirection == -1)
            {
                facingDirection *= -1;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                transform.position = transform.position + new Vector3(-0.5f, 0);
            }
        }
    }

    // Purchase missile
    public void AttemptPurchaseMissile(string missile)
    {
        bool purchased = missileManager.MissileRequest(turnPoints, missile);
        if (purchased && missileManager.missileObjects[missile] != currentMissile)
        {
            // Charge them + refund some
            turnPoints -= missileManager.missiles[missileManager.missileObjects[missile]];

            // Give player missile if can afford and not same
            currentMissile = missileManager.missileObjects[missile];
            currentMissileName = missile;
        }
    }

    // Manage turn points
    private void ManageTurnPoints()
    {
        // Get points if haven't already
        if (playerTurn && !hasEarnedPoints)
        {
            turnPoints += 50;
            hasEarnedPoints = true;
        }
        // If its the other players turn, reset so we can earn next turn
        if (!playerTurn)
        {
            hasEarnedPoints = false;
            currentMissile = missileManager.missileObjects["Basic Missile"];
            currentMissileName = "Basic Missile";
        }
    }

    // Choose missile
    //Pretty sure we can delete this now
    private void ChooseMissile()
    {
        if (playerTurn && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AttemptPurchaseMissile("Missile 1");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AttemptPurchaseMissile("Missile 2");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AttemptPurchaseMissile("Missile 3");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AttemptPurchaseMissile("Missile 4");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                AttemptPurchaseMissile("Missile 5");
            }
        }
    }

    // Display last shot position
    private void DisplayLastShotPos()
    {
        lastShotMarker = Instantiate(ShotMarker, lastShot, Quaternion.identity);
        if (turnManager.IsPlayerTurn(1))
        {
            lastShotMarker.GetComponent<SpriteRenderer>().color = new Color(0f,.4f,.1f,1f);
        }
        if (turnManager.IsPlayerTurn(2))
        {
            lastShotMarker.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,0.75f);
        }
    }

    // Shoot your gun
    private void Shoot()
    {
        // You can only shoot if buy menu isn't up
        buyMenu = GameObject.Find("Container");


        if (Input.GetKeyDown(KeyCode.Mouse0)
            && GameObject.FindGameObjectWithTag("Projectile") == null
            && playerTurn
            && !gameOverScreen.GameOver
            && !testing
            && !buyMenu
            && !EventSystem.current.IsPointerOverGameObject())
        {
            // Get rid of the aiming arrow and last shot marker
            Destroy(FindObjectOfType<Arrow>().gameObject);
            Destroy(lastShotMarker);

            // Update last shot position and make sure z coordinate is 1
            lastShot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastShot.z = 1;
            DisplayLastShotPos();

            // Find mouse position relative to tank position
            Vector3 relativeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            // Calculate rotation to spawn with
            float yRel = Mathf.Clamp(relativeMousePos.y, 0.025f, 12.5f); // Can't shoot down
            float xRel = Mathf.Clamp(relativeMousePos.x, 0.025f, 12.5f);
            if (facingDirection == 1 && turnManager.IsPlayerTurn(1))
            {
                xRel = Mathf.Clamp(relativeMousePos.x, 0.025f, 12.5f);
            }
            else if(turnManager.IsPlayerTurn(1))
            {
                xRel = Mathf.Clamp(relativeMousePos.x, -12.5f, -0.025f);
            }
            if (facingDirection == 1 && turnManager.IsPlayerTurn(2))
            {
                xRel = Mathf.Clamp(relativeMousePos.x, -12.5f, -0.025f);
            }
            else if (turnManager.IsPlayerTurn(2))
            {
                xRel = Mathf.Clamp(relativeMousePos.x, 0.025f, 12.5f);
            }
            var angle = Mathf.Atan2(yRel, xRel) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            // Spawn missile in direction of arrow
            Vector3 normalized = new Vector3(xRel, yRel, 0).normalized;
            Vector3 missilePos = transform.position + normalized * 2.5f;
            GameObject missile = Instantiate(currentMissile, missilePos, q);

            // Vector to scale velocity
            Vector3 addVelo = new Vector3(xRel, yRel, 0.0f);

            // Add velocity to the missile
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * addVelo;

            // Change turns
            turnManager.GetComponent<TurnManager>().ChangeTurns();
        }
    }
}
