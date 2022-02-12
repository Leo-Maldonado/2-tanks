using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank1 : MonoBehaviour
{
    // Which tanks turn it is to shoot
    public PlayersTurn playerTurn = PlayersTurn.Tank1;
    public enum PlayersTurn { Tank1, Tank2 }

    // Missile
    public GameObject missilePrefab;

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

    public enum BulletType { Bullet1, Bullet2, Bullet3 };

    public BulletType bullet = BulletType.Bullet1;
    // Apply the specified damage to the tank's health
    public void TakeDamage(int damage)
    {
        this.Health -= damage;
    }

    // Start
    void Start()
    {
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTurn == PlayersTurn.Tank1 && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.bullet = BulletType.Bullet1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.bullet = BulletType.Bullet2;

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                this.bullet = BulletType.Bullet3;

            }
        }
    


        // Display last shot position
        if (GameObject.FindGameObjectWithTag("Projectile") == null
            && playerTurn == PlayersTurn.Tank1
            && !gameOverScreen.GameOver
            && lastShot.z > 0
            && ! lastShotMarker)
        {
            lastShotMarker = Instantiate(ShotMarker, lastShot, Quaternion.identity);
        }
        // Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0)
            && GameObject.FindGameObjectWithTag("Projectile") == null
            && playerTurn == PlayersTurn.Tank1
            && !gameOverScreen.GameOver
            )
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
            GameObject missile = Instantiate(missilePrefab, missilePos, Quaternion.identity);
            var normalizedPos = relativeMousePos.normalized;
            float rotation = Mathf.Atan2(normalizedPos.y, normalizedPos.x) * Mathf.Rad2Deg;
            //missile.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rotation * -1);
            // Add velocity to the missile
            missile.GetComponent<Rigidbody2D>().velocity = missileVelocity * relativeMousePos;
            if(this.bullet == BulletType.Bullet1)
            {
                missile.GetComponent<Bullet>().bullet = Bullet.BulletType.Bullet1;
            }
            if (this.bullet == BulletType.Bullet2)
            {
                missile.GetComponent<Bullet>().bullet = Bullet.BulletType.Bullet2;
            }
            if (this.bullet == BulletType.Bullet3)
            {
                missile.GetComponent<Bullet>().bullet = Bullet.BulletType.Bullet3;
            }


            playerTurn = PlayersTurn.Tank2;
        }
        // If we lost
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
}