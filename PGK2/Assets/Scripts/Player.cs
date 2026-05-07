using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor, PlayerControls.IPlayerInputActions
{
    public UI ui;

    private WeaponContainer weaponContainer;
    public Weapon weapon;

    // Usuñ InputActionReference; korzystamy teraz z PlayerControls przechowywanego w InputManager
    private PlayerControls controls;

    // lokalne buforowanie wejœcia aktualizowanego przez callbacki
    private Vector2 cachedMovement = Vector2.zero;
    private Vector2 cachedMouseScreenPos = Vector2.zero;

    public State state;

    public GameEvent onPlayerDashStart;
    public GameEvent onPlayerDashEnd;
    public GameEvent PlayerHealthChange;


    public float dashRadius;
    public float dashCooldown;
    private float dashLastUsed;


    public bool isWeaponDash;
    public bool isAttacking;
    public bool isDashing;


    public override int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            ui.UpdateHealthBar();
        }
    }
    protected override void Start()
    {
        base.Start();

        weaponContainer = GameManager.instance.weaponContainer;
        weapon = GameManager.instance.weapon;
        ui = GameManager.instance.ui;
        state = State.Idle;


        dashRadius = 2.0f;
        dashCooldown = 2.0f;
        dashLastUsed = -dashCooldown;
        isAttacking = false;
        isDashing = false;
        isWeaponDash = false;
        //MaxHealth = 10;

        InitializeHealth();

        
        controls = InputManager.Instance.Controls;
    }
    private void FixedUpdate()
    {
        weaponContainer.PointerPosition = GetMousePos();
        Moving();
    }
    private Vector2 GetMousePos()
    {
        // Konwertuj zbuforowan¹ pozycjê myszy (ekranow¹) na world point
        if (Camera.main == null)
            return cachedMouseScreenPos;
        return Camera.main.ScreenToWorldPoint(cachedMouseScreenPos);
    }
    private void SwapSpriteDirection()
    {
        if (isAttacking)
            return;

        Vector2 mousePos = GetMousePos();
        if (transform.position.x < mousePos.x)
            spriteRenderer.flipX = false;
        else if (transform.position.x > mousePos.x)
            spriteRenderer.flipX = true;
    }

    private void OnEnable()
    {
        controls = InputManager.Instance.Controls;
        controls.PlayerInput.AddCallbacks(this);
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.PlayerInput.RemoveCallbacks(this);
    }

    // Implementacja interfejsu wygenerowanego przez PlayerControls
    public void OnMovement(InputAction.CallbackContext context)
    {
        // Aktualizuj bufor ruchu (wykorzystywane w Movement())
        cachedMovement = context.ReadValue<Vector2>();
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        // Buforuj pozycjê myszy w przestrzeni ekranu
        cachedMouseScreenPos = context.ReadValue<Vector2>();
    }

    public void OnAttackLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackLeft();
    }

    public void OnAttackRight(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackRight();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnDash();
    }

    public void OnInventoryOn(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInventoryOn();
    }

    private void OnInventoryOn()
    {
        ui.EquipmentToggle();
    }

    public void OnAttackLeft()
    {
        weapon.Attack(true);
    }
    public void OnAttackRight()
    {
        weapon.Attack(false);
    }
    public void OnDash()
    {
        if (isDashing)
        {
            Debug.Log("already dashing");
            return;
        }

        onPlayerDashStart.Raise(this, null);

        // cooldown
        if (!isWeaponDash)
        { 
            if (Time.time - dashLastUsed < dashCooldown)
                return;
        }
        

        isDashing = true;
        animator.SetBool("isDashing", isDashing);
        state = State.Dash;

        // sprite aplha
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        weapon.spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        // weapon container stop rotation
        weaponContainer.enabled = false;

        // weapon state
        weapon.weaponState = WeaponState.Dashing;

        // get vector2 of mouse position to dash to
        moveDirection = GetMousePos() - (Vector2)transform.position;
        moveDirection.Normalize();
        moveDirection.x *= dashRadius;
        moveDirection.y *= dashRadius;
    }
    public void OnDashLasting() 
    {
        // Try to move player in input direction, followed by left right and up down input if failed
        bool success = CollisionCheck(moveDirection);

        if (!success)
        {
            // Try Left / Right
            CollisionCheck(new Vector2(moveDirection.x, 0));

            // Try UP / Down
            CollisionCheck(new Vector2(0, moveDirection.y));
        }
    }
    public void OnDashEnd()
    {
        isDashing = false;
        animator.SetBool("isDashing", isDashing);
        state = State.Idle;

        onPlayerDashEnd.Raise(this, null);

        // set cooldown if the dash comes from player not the weapon
        if (!isWeaponDash) {dashLastUsed = Time.time;}
            
        // weapon animator dash
        weapon.animator.SetBool("isDashing", isDashing);

        // sprite aplha
        spriteRenderer.color = new Color(1, 1, 1, 1);
        weapon.spriteRenderer.color = new Color(1, 1, 1, 1);

        // weapon container stop rotation
        weaponContainer.enabled = true;

        // weapon state
        weapon.weaponState = WeaponState.Idle;

        // weapon dash reset
        isWeaponDash = false;
    }
    private void Moving()
    {
        if (isDashing)
            OnDashLasting();
        else
            Movement();
    }
    public override void Movement()
    {
        if (state == State.Dash)
            return;

        moveDirection = cachedMovement.normalized;
        // Try to move player in input direction, followed by left right and up down input if failed
        bool success = CollisionCheck(moveDirection);

        if (!success)
        {
            // Try Left / Right
            CollisionCheck(new Vector2(moveDirection.x, 0));

            // Try UP / Down
            CollisionCheck(new Vector2(0, moveDirection.y));
        }

        // Animation swap between idle and walk 
        if (moveDirection != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            state = State.Walk;
        }
        else
        {
            animator.SetBool("isMoving", false);
            state = State.Idle;
        }

        SwapSpriteDirection();
    }

    public override void TakeDamage(DamageStruct damageStruct)
    {
        if (isDashing)
            return;
        base.TakeDamage(damageStruct);
    }

}



public enum State 
{
    Idle,
    Walk,
    Dash
}
