using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public int maxHealth;
    public virtual int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        { 
            maxHealth = value;
        }
    }

    public int currentHealth;
    public virtual int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }


    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D collider;
    public ContactFilter2D movementFilter;

    private readonly List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public Vector2 moveDirection;

    public float moveSpeed;
    public float collisionOffset;
    

    protected virtual void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        collisionOffset = 0.12f;
    }
    public virtual void InitializeHealth()
    {
        CurrentHealth = MaxHealth;
    }
    public virtual void TakeDamage(DamageStruct damageMessage)
    {
        float damage = damageMessage.damage;

        // check if hit is critical
        if (damageMessage.criticalChance >= Random.Range(1, 101))
        {
            damage *= damageMessage.criticalDamage;
        }

        // TODO: apply status

        CurrentHealth -= (int)damage;
        Debug.Log($"Damage dealt to {gameObject.name} is {(int)damage}");

        if (CurrentHealth <= 0)
        {
            Death();
        }
    }
    public virtual void Death() 
    {
        Destroy(gameObject);
    }

    public bool CollisionCheck(Vector2 direction)
    {
        // Check for potential collisions 
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
            castCollisions, // List of collisions to store the found collisions into after the Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movementInput plus an offset


        if (count == 0)
        {
            Vector2 moveVector = moveSpeed * Time.fixedDeltaTime * direction;

            // No collisions
            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Movement()
    {

    }
}

