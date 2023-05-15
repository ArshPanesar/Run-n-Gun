using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Shooting
    public BulletPreset bulletPreset;
    private Vector2 lineOfShot = Vector2.left;

    public float activeShootTime = 0.9f;
    public float waitShootTime = 0.5f;
    private float shootTimer = 0.0f;
    enum State
    {
        ACTIVE = 0,
        WAITING,
        COMPUTING // To Compute Line of Shot
    };
    private State state = State.ACTIVE;


    public float maxAngleWithLeftShot = 60f;
    public float maxAngleWithUpShot = 30f;
    public float maxAngleWithRightShot = 60f;

    // Components
    private VisibilityNotifierTarget visibilityNotifierTarget;
    private BulletSpawner bulletSpawner;

    // Player Location
    private GameObject playerObject = null;
    private bool isPlayerVisible = false;

    private void PlayerVisible()
    {
        playerObject = visibilityNotifierTarget.sourceObject;
        isPlayerVisible = true;
    }

    private void Start()
    {
        visibilityNotifierTarget = GetComponent<VisibilityNotifierTarget>();
        bulletSpawner = GetComponent<BulletSpawner>();

        // Set Delegate Handle
        visibilityNotifierTarget.delegateHandle += PlayerVisible; 
    }

    private void Update()
    {
        if (!isPlayerVisible)
        {
            return;
        }

        shootTimer += Time.deltaTime;
        
        // Do as per the current state
        switch (state)
        {
            case State.ACTIVE:
                {
                    // Shoot
                    bulletSpawner.spawnPosition = Vector2.zero;
                    bulletSpawner.lineOfShot = lineOfShot;
                    bulletSpawner.Spawn(bulletPreset, LayerMask.NameToLayer("EnemyBullet"));

                    // Check State
                    if (shootTimer > activeShootTime)
                    {
                        state = State.WAITING;
                        shootTimer = 0.0f;
                    }
                }
                break;

            case State.WAITING:
                {
                    // Check State
                    if (shootTimer > waitShootTime)
                    {
                        state = State.COMPUTING;
                        shootTimer = 0.0f;
                    }
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
                    }
                    else if (angleWithUp < maxAngleWithUpShot)
                    {
                        lineOfShot = Vector2.up;
                    }
                    else if (angleWithRight < maxAngleWithRightShot)
                    {
                        lineOfShot = Vector2.right;
                    }

                    // Change State
                    state = State.ACTIVE;
                    shootTimer = 0.0f;
                }
                break;
        }
    }
}
