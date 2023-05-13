using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    public float speed = 1f;
    public float jumpForce = 4f;
    public int ignoreJumpFrames = 4;

    // Components
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;

    // Input
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool alreadyJumping = false;
    private bool shouldJump = false;

    // Movement
    private int countFrames = 0;
    private Vector2 moveVelocity = new Vector2();

    private float distToFloor = 0f;

    // Score
    int score = 0;

    // Check if Player is Standing on a Floor
    bool IsOnFloor()
    {
        RaycastHit2D raycastResult = Physics2D.Raycast(transform.position, -Vector2.up, distToFloor, LayerMask.GetMask("Floor"));
        if (raycastResult.collider != null)
            return true;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set Up Components
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        distToFloor = boxCollider.bounds.extents.y + (boxCollider.bounds.extents.y * 0.1f);
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
    }

    void FixedUpdate()
    {
        if (moveLeft)
        {
            moveVelocity.x = -speed;
        }
        else if (moveRight)
        {
            moveVelocity.x = speed;
        }
        else
        {
            moveVelocity.x = 0f;
        }
        
        if (!alreadyJumping && shouldJump && IsOnFloor())
        {
            shouldJump = false;
            alreadyJumping = true;
        }
        else if (alreadyJumping)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));

            countFrames++;
            if (countFrames > ignoreJumpFrames - 1)
            {
                alreadyJumping = false;
                countFrames = 0;
            }
        }
        moveVelocity.y = rigidBody.velocity.y;

        // Apply Velocity
        rigidBody.velocity = moveVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        moveLeft = Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.D);
        shouldJump = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
    }

    // Increase Score
    void IncrementScore()
    {
        score += 1;
    }
}
