using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBehaviour : MonoBehaviour
{
    // Health
    public int health = 100;
    public float waitBeforeDestroyTime = 0.5f;
    private float destroyTimer = 0.0f;

    // Shooting
    public BulletPreset bulletPreset;
    private Vector2 spawnPoint = Vector2.zero;
    private Vector2 lineOfShot = Vector2.left;

    public Vector2 bulletSpawnOffset;
    public Vector2 bulletUpSpawnOffset;

    public float initTime = 0.0f;
    public float activeShootTime = 0.9f;
    public float waitShootTime = 0.5f;
    private float shootTimer = 0.0f;
    enum State
    {
        INIT = 0,
        ACTIVE,
        WAITING,
        COMPUTING // To Compute Line of Shot
    };
    private State state = State.INIT;

    public float maxAngleWithLeftShot = 60f;
    public float maxAngleWithUpShot = 30f;
    public float maxAngleWithRightShot = 60f;

    // Components
    private VisibilityNotifierTarget visibilityNotifierTarget;
    private BulletSpawner bulletSpawner;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Player Location
    private GameObject playerObject = null;
    private bool isPlayerVisible = false;

    private void PlayerVisible(GameObject sourceObject)
    {
        if (sourceObject.tag == "Player")
        {
            playerObject = sourceObject;
            isPlayerVisible = true;
        }
    }
    private void PlayerLeft(GameObject sourceObject)
    {
        if (sourceObject.tag == "Player")
        {
            isPlayerVisible = false;
        }
    }

    private void PlayerDead(Dictionary<string, object> args)
    {
        isPlayerVisible = false;
        animator.SetBool("PlayerDead", true);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        // Play Hit Sound
        FindObjectOfType<AudioManager>().Play("Enemy Hit");

        if (health <= 0)
        {
            // Remove Collision
            GetComponent<BoxCollider2D>().enabled = false;

            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = 0f;
            rigidBody.MovePosition(transform.position + new Vector3(0f, -0.20f));
            
            // Animate Death
            animator.SetBool("IsDead", true);

            // Play Death Sound
            FindObjectOfType<AudioManager>().Play("Enemy Death");
        }
    }

    private void Start()
    {
        visibilityNotifierTarget = GetComponent<VisibilityNotifierTarget>();
        bulletSpawner = GetComponent<BulletSpawner>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        visibilityNotifierTarget.delegateHandle += PlayerVisible;
        visibilityNotifierTarget.exitDelegateHandle += PlayerLeft;

        // Check if in Group
        EnemyGroupBehaviour parentGroup = gameObject.GetComponentInParent<EnemyGroupBehaviour>();
        if (parentGroup != null)
        {
            parentGroup.SubscribeToNotification(PlayerVisible);
        }

        EventManager.GetInstance().AddListener(GameEvents.PlayerDead, PlayerDead);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.PlayerDead, PlayerDead);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;
        if (otherObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            BulletBehaviour bulletBehaviour = otherObject.GetComponent<BulletBehaviour>();
            TakeDamage(bulletBehaviour.damage);
        }
    }

    private void Update()
    {
        if (!isPlayerVisible)
        {
            return;
        }
        // Wait Before Destroying
        if (health <= 0)
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer > waitBeforeDestroyTime)
            {
                Destroy(gameObject);
            }

            return;
        }

        shootTimer += Time.deltaTime;
        
        // Do as per the current state
        switch (state)
        {
            case State.INIT:
                {
                    if (shootTimer > initTime) 
                    {
                        state = State.COMPUTING;
                        shootTimer = 0.0f;
                    }
                }
                break;

            case State.ACTIVE:
                {
                    // Shoot
                    bulletSpawner.spawnPosition = spawnPoint;
                    bulletSpawner.lineOfShot = lineOfShot;
                    bulletSpawner.Spawn(bulletPreset, LayerMask.NameToLayer("EnemyBullet"));

                    // Check State
                    bool switchState = shootTimer > activeShootTime;
                    if (switchState)
                    {
                        state = State.WAITING;
                        shootTimer = 0.0f;
                    }

                    // Animation
                    string conditionName = "IsShooting";
                    if (lineOfShot == Vector2.up)
                    {
                        conditionName = "IsShootingUp";
                    }
                    animator.SetBool(conditionName, !switchState);
                }
                break;

            case State.WAITING:
                {
                    // Check State
                    bool switchState = shootTimer > waitShootTime;
                    if (switchState)
                    {
                        state = State.COMPUTING;
                        shootTimer = 0.0f;
                    }

                    // Animation
                    string conditionName = "IsWaiting";
                    if (lineOfShot == Vector2.up)
                    {
                        conditionName = "IsWaitingUp";
                    }
                    animator.SetBool(conditionName, !switchState);
                }
                break;

            case State.COMPUTING:
                {
                    // Find out which direction to shoot
                    Vector2 playerToEnemyDir = playerObject.transform.position - transform.position;

                    float angleWithLeft = Vector2.Angle(playerToEnemyDir, Vector2.left);
                    float angleWithUp = Vector2.Angle(playerToEnemyDir, Vector2.up);
                    float angleWithRight = Vector2.Angle(playerToEnemyDir, Vector2.right);

                    if (angleWithLeft < maxAngleWithLeftShot)
                    {
                        lineOfShot = Vector2.left;

                        spawnPoint = new Vector2(-bulletSpawnOffset.x, bulletSpawnOffset.y);
                    }
                    else if (angleWithRight < maxAngleWithRightShot)
                    {
                        lineOfShot = Vector2.right;

                        spawnPoint = bulletSpawnOffset;
                    }
                    else if (angleWithUp < maxAngleWithUpShot)
                    {
                        lineOfShot = Vector2.up;

                        spawnPoint = bulletUpSpawnOffset;
                    }

                    // Flip Sprite
                    spriteRenderer.flipX = (lineOfShot == Vector2.left);

                    // Change State
                    state = State.ACTIVE;
                    shootTimer = 0.0f;
                }
                break;
        }
    }
}
