using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    // Debug Options
    //
    // Player can't die
    public bool dGodMode = false;
    public BulletPreset dBulletPreset;

    // Health
    public int health = 100;
    public int healthPickupValue = 50;
    public float fallDeathHeight = -5;

    // Movement
    public float speed = 1f;
    public float jumpForce = 4f;
    public int ignoreJumpFrames = 4;

    // Components
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    private BulletSpawner bulletSpawner;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Input
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool alreadyJumping = false;
    private bool shouldJump = false;
    private bool canShoot = false;

    // Jumping
    public Transform floorDetectorTransform;
    private int countFrames = 0;
    private float distToFloor = 0f;
    private bool wasOnFloorPrevFrame = false;

    // Score
    int score = 0;

    // Check if Player is Standing on a Floor
    bool IsOnFloor()
    {
        RaycastHit2D raycastResult = Physics2D.Raycast(floorDetectorTransform.position, -Vector2.up, distToFloor, LayerMask.GetMask("Floor", "Platform"));
        if (raycastResult.collider != null)
        {
            return true;
        }
        return false;
    }

    void Shoot()
    {
        BulletPreset bulletPreset = BulletPresetContainer.Get("DefaultBullet");
        if (dBulletPreset != null)
        {
            bulletPreset = dBulletPreset;
        }

        Vector2 spawnPoint = new Vector2(0.81f, 0.32f);
        bulletSpawner.spawnPosition = spawnPoint;
        bulletSpawner.lineOfShot = Vector2.right;
        if (spriteRenderer.flipX)
        {
            spawnPoint.x = -spawnPoint.x;
            bulletSpawner.spawnPosition = spawnPoint;
            bulletSpawner.lineOfShot = Vector2.left;
        }

        bulletSpawner.Spawn(bulletPreset, LayerMask.NameToLayer("PlayerBullet"));
    }

    void TakeDamage(int damage)
    {
        if (dGodMode)
        {
            return;
        }

        health -= damage;

        if (health <= 0)
        {
            health = 0;

            // Correct Death Position
            boxCollider.enabled = false;
            rigidBody.gravityScale = 0f;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.position += new Vector3(0f, -0.40f);

            EventManager.GetInstance().TriggerEvent(GameEvents.PlayerDead, null);

            // Animate Death
            animator.SetBool("IsDead", true);
        }

        EventManager.GetInstance().TriggerEvent(GameEvents.UpdateHealth, new Dictionary<string, object>
        {
            { "health", health }
        });
    }

    void Start()
    {
        // Set Up Components
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponent<BulletSpawner>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //distToFloor = boxCollider.bounds.extents.y + (boxCollider.bounds.extents.y * 0.1f);
        distToFloor = (boxCollider.bounds.extents.y * 0.1f);

        EventManager.GetInstance().TriggerEvent(GameEvents.UpdateHealth, new Dictionary<string, object>
        {
            { "health", health }
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collided with a Coin
        if (collision.tag == "Coin")
        {
            IncrementScore();

            EventManager.GetInstance().TriggerEvent(GameEvents.CollectCoins, new Dictionary<string, object>{
                { "coin", collision.gameObject },
                { "score", score }
            });
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            BulletBehaviour bulletBehaviour = collision.gameObject.GetComponent<BulletBehaviour>();
            TakeDamage(bulletBehaviour.damage);
        }
        else if (collision.gameObject.tag == "HealthPickup")
        {
            Destroy(collision.gameObject);

            health += healthPickupValue;

            EventManager.GetInstance().TriggerEvent(GameEvents.UpdateHealth, new Dictionary<string, object>{
                { "health", health }
            });
        }
    }

    void FixedUpdate()
    {
        if (health <= 0)
        {
            return;
        }

        if (moveLeft)
        {
            // Physics
            rigidBody.AddForce(new Vector2(-speed, 0f), ForceMode2D.Impulse);

            // Graphics
            spriteRenderer.flipX = true;
            animator.SetBool("IsMoving", true);
        }
        else if (moveRight)
        {
            // Physics
            rigidBody.AddForce(new Vector2(speed, 0f), ForceMode2D.Impulse);

            // Graphics
            spriteRenderer.flipX = false;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            // Physics
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);

            // Graphics
            animator.SetBool("IsMoving", false);
        }

        // Jumping: Physics
        bool isOnFloor = IsOnFloor();
        if (!alreadyJumping && shouldJump && isOnFloor)
        {
            shouldJump = false;

            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            alreadyJumping = true;
        }
        else if (alreadyJumping && isOnFloor)
        {
            //rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

            alreadyJumping = false;

            countFrames++;
            if (countFrames > ignoreJumpFrames - 1)
            {
                alreadyJumping = false;
                countFrames = 0;
            }
        }

        // Jumping: Graphics
        animator.SetBool("IsJumping", !isOnFloor);

        // Check if Falling to Death
        if (transform.position.y <= fallDeathHeight)
        {
            TakeDamage(health);
        }

        // Apply Velocity
        rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -speed, speed), 
                                         Mathf.Clamp(rigidBody.velocity.y, -jumpForce, jumpForce));
    }

    void Update()
    {
        if (health <= 0)
        {
            return;
        }

        moveLeft = Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.D);
        shouldJump = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
        canShoot = Input.GetKey(KeyCode.Return);
        
        if (canShoot)
        {
            Shoot();
        }

        animator.SetBool("IsShooting", canShoot);
    }

    // Increase Score
    void IncrementScore()
    {
        score += 1;
    }
}
