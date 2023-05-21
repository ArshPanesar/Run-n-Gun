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
    
    public bool takeEnemyCollisionDamage = true;
    public int enemyCollisionDamage = 15;
    public float enemyCollisionDamageTimeout = 0.5f;
    private float enemyCollisionDamageTimer = 0.0f;

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
    private float distToFloor = 0f;
    
    // Score
    int score = 0;
    int currentCoins = 0;

    // Weapon Upgrades
    public PlayerBulletUpgrade[] bulletUpgradeList;
    public int currentBulletUpgradeIndex = 0;

    // UI
    private bool paused = false;
    private bool upgradingWeapon = false;
    private bool levelFinished = false;

    public void UpgradeWeapon()
    {
        int nextUpgrade = currentBulletUpgradeIndex + 1;
        if (nextUpgrade >= bulletUpgradeList.Length)
        {
            return;
        }

        if (currentCoins < bulletUpgradeList[nextUpgrade].coinsRequired)
        {
            return;
        }

        currentCoins -= bulletUpgradeList[nextUpgrade].coinsRequired;
        currentBulletUpgradeIndex = nextUpgrade;

        FindObjectOfType<AudioManager>().Play("Upgrade Weapon");

        // Update Score
        EventManager.GetInstance().TriggerEvent(GameEvents.CollectCoins, new Dictionary<string, object>{
                { "coin", null },
                { "score", currentCoins }
            });

        // Remove Weapon Upgrade Prompt
        EventManager.GetInstance().TriggerEvent(GameEvents.WeaponUpgraded, null);

        upgradingWeapon = false;
    }

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
        // Pick Bullet According to Current Upgrade
        BulletPreset bulletPreset = bulletUpgradeList[currentBulletUpgradeIndex].bulletPreset;
        
        // Debug Mode Preset
        if (dBulletPreset != null)
        {
            bulletPreset = dBulletPreset;
        }

        // Spawn the Bullet
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

        // Play Hit Sound
        FindObjectOfType<AudioManager>().Play("Player Hit");

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

            // Play Death Sound
            FindObjectOfType<AudioManager>().Play("Player Death");
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

        distToFloor = (boxCollider.bounds.extents.y * 0.225f);

        EventManager.GetInstance().AddListener(GameEvents.Unpaused, OnUnpause);

        EventManager.GetInstance().TriggerEvent(GameEvents.UpdateHealth, new Dictionary<string, object>
        {
            { "health", health }
        });
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.Unpaused, OnUnpause);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collided with a Coin
        if (collision.tag == "Coin")
        {
            IncrementScore();

            EventManager.GetInstance().TriggerEvent(GameEvents.CollectCoins, new Dictionary<string, object>{
                { "coin", collision.gameObject },
                { "score", currentCoins }
            });
        }
        // Enemy Bullet
        else if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            BulletBehaviour bulletBehaviour = collision.gameObject.GetComponent<BulletBehaviour>();
            TakeDamage(bulletBehaviour.damage);
        }
        // Health Pickup
        else if (collision.gameObject.tag == "HealthPickup")
        {
            Destroy(collision.gameObject);

            health += healthPickupValue;

            FindObjectOfType<AudioManager>().Play("Health Pickup");

            EventManager.GetInstance().TriggerEvent(GameEvents.UpdateHealth, new Dictionary<string, object>{
                { "health", health }
            });
        }
        // Finish Flag
        else if (collision.gameObject.tag == "Finish")
        {
            levelFinished = true;

            EventManager.GetInstance().TriggerEvent(GameEvents.FinishLevel, new Dictionary<string, object>
            {
                { "coins", score }
            });
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        enemyCollisionDamageTimer += Time.deltaTime;
        
        Collider2D collider = collision.collider;
        if (collider.gameObject.tag == "Enemy")
        {
            if (takeEnemyCollisionDamage)
            {
                TakeDamage(enemyCollisionDamage);

                takeEnemyCollisionDamage = false;
            }
            else if (enemyCollisionDamageTimer > enemyCollisionDamageTimeout)
            {
                takeEnemyCollisionDamage = true;
                enemyCollisionDamageTimer = 0.0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (health <= 0 || levelFinished)
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
        else if (isOnFloor)
        {
            alreadyJumping = false;
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
        if (health <= 0 || levelFinished)
        {
            return;
        }

        // Pause the Game
        if (!upgradingWeapon && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            paused = !paused;
            EventManager.GetInstance().TriggerEvent(GameEvents.Paused, null);
        }

        // Upgrade Weapon
        if (!paused && Input.GetKeyDown(KeyCode.B))
        {
            upgradingWeapon = !upgradingWeapon;

            int nextUpgrade = (currentBulletUpgradeIndex + 1) % bulletUpgradeList.Length;
            EventManager.GetInstance().TriggerEvent(GameEvents.UpgradeWeaponMenu, new Dictionary<string, object> { 
                { "coinsCollected", currentCoins },
                { "coinsRequired", bulletUpgradeList[nextUpgrade].coinsRequired },
                { "maxUpgraded", (currentBulletUpgradeIndex + 1 == bulletUpgradeList.Length) },
                { "bulletAnimatorCont", bulletUpgradeList[nextUpgrade].bulletPreset.animationController},
                { "playerBehaviour", this }
            });
        }

        // Take Input from User
        moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        shouldJump = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow);
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
        ++score;
        ++currentCoins;

        // Check if Weapon can be Upgraded
        int nextUpgrade = currentBulletUpgradeIndex + 1;
        if (nextUpgrade < bulletUpgradeList.Length)
        {
            if (currentCoins >= bulletUpgradeList[nextUpgrade].coinsRequired)
            {
                EventManager.GetInstance().TriggerEvent(GameEvents.CanUpgradeWeapon, null);
            }
        }

        // Play Sound
        FindObjectOfType<AudioManager>().Play("Coin");
    }

    void OnUnpause(Dictionary<string, object> args)
    {
        paused = false;
    }
}
