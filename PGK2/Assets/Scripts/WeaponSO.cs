using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon template", menuName = "ScriptableObjects/Weapon Template")]
public class WeaponSO : ScriptableObject
{
    public Sprite sprite;
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
}
public enum WeaponType
{
    Sword,
    Longsword,
    Widesword,
    Katana,
    Dagger,
    Rapier,
    Spear,
    Halberd,
    Scythe,
    Khopesh,
    Axe,
    SmallHammer,
    BigHammer,
    Club,
    Mace,
    Trident,
    Shield,
    Wand
}
public enum AttackType
{
    Swing, // 0

    Push, // 1
    MultiPush, // 2
    DashPush, // 3
    PushBerserk, // 4

    DoubleSwing, // 5
    GroundHit, // 6

    DoubleTipHit, // 7
    DoubleTipHitBerserk, // 8

    RotationSwing, // 9
    RotationInFront, // 10

    Throw // 11
}