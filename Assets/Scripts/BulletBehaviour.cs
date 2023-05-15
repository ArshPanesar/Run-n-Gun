using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float speed;
    private Vector2 direction;
    
    public int damage;

    public void SetBullet(float newSpeed, Vector2 newDirection, int newDamage, RuntimeAnimatorController animController)
    {
        speed = newSpeed;
        direction = newDirection;
        damage = newDamage;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.runtimeAnimatorController = animController;
        
        // Flip Sprite according to Direction
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        float angleWithLeft = Vector2.Angle(direction, Vector2.left);
        float angleWithRight = Vector2.Angle(direction, Vector2.right);
        if (angleWithLeft < angleWithRight)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventManager.GetInstance().TriggerEvent(GameEvents.DestroyBullet, new Dictionary<string, object>
            {
                { "bullet", gameObject }
            });
    }

    private void FixedUpdate()
    {
        // Move
        transform.position = new Vector2(transform.position.x, transform.position.y) + (direction * speed) * Time.fixedDeltaTime;
    }

    private void OnBecameInvisible()
    {
        EventManager.GetInstance().TriggerEvent(GameEvents.DestroyBullet, new Dictionary<string, object>
            {
                { "bullet", gameObject }
            });
    }
}
