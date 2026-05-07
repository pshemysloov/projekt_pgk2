using System;
using UnityEngine;


public class EnemyHitbox : MonoBehaviour
{
    EnemyAI owner;
    private Boolean didHit;
    private void Awake()
    {
        owner = GetComponentInParent<EnemyAI>();
    }

    private void OnEnable()
    {
        didHit = false;
        Debug.Log("Hitbox enabled");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (didHit == true)
        {
            return;
        }
        if (collision.gameObject.tag == "Player" && didHit == false)
        {
            DamageStruct damageMessage = new()
            {
                damage = owner.damage,
                criticalChance = owner.criticalChance,
                criticalDamage = owner.criticalDamage,
                statusChance = owner.statusChance,
            };

            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damageMessage);
            didHit = true;
        }
        gameObject.SetActive(false);
    }
}
