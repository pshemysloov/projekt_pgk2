using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy template", menuName = "ScriptableObjects/Enemy Template")]
public class EnemySO : ScriptableObject
{
    public int maxHealth;

    public float moveSpeed;
    public float collisionOffset;

    public float detectionDelay = 0.05f;
    public float aiUpdateDelay = 0.06f;
    public float attackDelay = 1f;
    public float attackDistance = 0.5f;

    public Vector3 projectileOffset;
    public float projectileSpeed;
    public float projectileDuration;

    public float damage;
    public float statusChance;
    public float criticalChance;
    public float criticalDamage;

    //sprite
    public RuntimeAnimatorController controller;

    //custom collider
    public Vector2 hitboxOffset;
    public Vector2 hitboxSize;
}
