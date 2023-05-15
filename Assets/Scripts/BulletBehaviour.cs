using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float speed;
    private Vector2 direction;
    private int damage;

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
                { "bullet", gameObject },
                { "damage", damage }
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
                { "bullet", gameObject },
                { "damage", damage }
            });
    }
}
