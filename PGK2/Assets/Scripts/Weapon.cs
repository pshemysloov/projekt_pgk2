using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : WeaponTemplate
{
    public Animator animator;
    public Player player;
    public WeaponSO weaponSO;
    public WeaponState weaponState;
    private WeaponContainer weaponContainer;
    public BoxCollider2D hitbox;

    //_______________Weapon template fields_________________
    public SpriteRenderer spriteRenderer;
    public int tier;
    public new string name;

    public float damage;

    public float statusChance;

    public float criticalChance;
    public float criticalDamage;

    public float range;

    public float cooldownSpeedMultiplier;
    public float attackSpeedMultiplier;
    public float cooldownSpeedMultiplierRight;
    public float attackSpeedMultiplierRight;

    public WeaponType weaponType;
    public AttackType attackType;
    public AttackType attackTypeRight;

    public int pushLoopCount;
    public float attackRightCooldown;
    public float attackRightLastUsed;
    //------------------------------------------------------

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameManager.instance.player;
        hitbox = GetComponentInChildren<BoxCollider2D>();
        weaponContainer = GameManager.instance.weaponContainer;
        weaponState = WeaponState.Idle;

        EquipWeapon();
    }
    public void EquipWeapon(WeaponSO weaponSO)
    {
        this.weaponSO = weaponSO;
        EquipWeapon();
    }

    public void EquipWeapon()
    {
        spriteRenderer.sprite = weaponSO.sprite;
        tier = weaponSO.tier;
        name = weaponSO.name;

        damage = weaponSO.damage;

        statusChance = weaponSO.statusChance;
        criticalChance = weaponSO.criticalChance;
        criticalDamage = weaponSO.criticalDamage;

        range = weaponSO.range;

        cooldownSpeedMultiplier = weaponSO.cooldownSpeedMultiplier;
        attackSpeedMultiplier = weaponSO.cooldownSpeedMultiplier;
        cooldownSpeedMultiplierRight = weaponSO.cooldownSpeedMultiplierRight;
        attackSpeedMultiplierRight = weaponSO.attackSpeedMultiplierRight;

        weaponType = weaponSO.weaponType;
        attackType = weaponSO.attackType;
        attackTypeRight = weaponSO.attackTypeRight;

        pushLoopCount = weaponSO.pushLoopCount;
        animator.SetInteger("pushLoopCount", pushLoopCount);

        attackRightCooldown = weaponSO.attackRightCooldown;
        attackRightLastUsed = -attackRightCooldown;

        // doing this to make sure animator refreshes animation set
        animator.SetBool("NewWeapon", true);
        animator.SetInteger("AttackTypeEnumMain", (int)attackType);
        animator.SetBool("NewWeapon", false);

        // hitbox position
        switch (weaponType)
        {
            case WeaponType.Sword:
                hitbox.offset = new Vector2(0.55f + range / 2, 0);
                hitbox.size = new Vector2(0.7f + range, 0.3f);
                break;
            case WeaponType.Longsword:
                break;
            case WeaponType.Widesword:
                break;
            case WeaponType.Katana:
                break;
            case WeaponType.Dagger:
                hitbox.offset = new Vector2(0.4f + range / 2, 0);
                hitbox.size = new Vector2(0.4f + range, 0.3f);
                break;
            case WeaponType.Rapier:
                hitbox.offset = new Vector2(0.55f + range / 2, 0);
                hitbox.size = new Vector2(0.7f + range, 0.3f);
                break;
            case WeaponType.Spear:
                break;
            case WeaponType.Halberd:
                break;
            case WeaponType.Scythe:
                break;
            case WeaponType.Khopesh:
                break;
            case WeaponType.Axe:
                break;
            case WeaponType.SmallHammer:
                break;
            case WeaponType.BigHammer:
                break;
            case WeaponType.Club:
                break;
            case WeaponType.Mace:
                break;
            case WeaponType.Trident:
                break;
            case WeaponType.Shield:
                break;
            case WeaponType.Wand:
                break;
            default:
                break;
        }
    }

    public void Attack(bool isLeft)
    {
        // dont attack if the weapon already attacks, is in cooldown or is during dash
        if (weaponState != WeaponState.Idle)
            return;

        // doing this to make sure animator refreshes animation set
        if (isLeft)
        { 
            animator.SetInteger("AttackTypeEnum", (int)attackType);
            SetAttackSpeed(attackSpeedMultiplier);
            SetCooldownSpeed(cooldownSpeedMultiplier);
        }
        else
        {
            // check cooldown for right attack
            if (Time.time - attackRightLastUsed < attackRightCooldown)
                return;

            animator.SetInteger("AttackTypeEnum", (int)attackTypeRight);
            SetAttackSpeed(attackSpeedMultiplierRight);
            SetCooldownSpeed(cooldownSpeedMultiplierRight);
            attackRightLastUsed = Time.time;
        }        
        animator.SetTrigger("Attack");
    }
    public void SetCooldownSpeed(float cooldown)
    {
        animator.SetFloat("AttackCooldownMultiplier", cooldown);
    }
    public void SetAttackSpeed(float speed)
    {
        animator.SetFloat("AttackSpeedMultiplier", speed);
    }
    public void SetWeaponState(WeaponState stateEnum)
    {
        weaponState = stateEnum;

        // Disable or enable weapon rotation
        if (weaponState == WeaponState.Attack)
        { 
            weaponContainer.enabled = false;
            player.isAttacking = true;
        }   
        else
        {
            weaponContainer.enabled = true;
            player.isAttacking = false;
        }
    }
    public void SetPushLoopCount()
    {
        int counter = animator.GetInteger("pushLoopCount");
        animator.SetInteger("pushLoopCount", counter-1);
    }
    public void ResetPushLoopCount()
    {
        animator.SetInteger("pushLoopCount", pushLoopCount);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DamageStruct damageMessage = new()
            {
                damage = damage,

                criticalChance = criticalChance,
                criticalDamage = criticalDamage,
                statusChance = statusChance,
            };

            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            enemy.TakeDamage(damageMessage);
        }
    }
}

public enum WeaponState
{ 
    Idle,
    Attack,
    Cooldown,
    Dashing
}
