using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float damage, statusChance, criticalChance, criticalDamage;

    private Vector2 moveVector;
    public void Attack(Transform target, Vector3 origin, float speed, float duration, float pdamage, float pstatusChance, float pcriticalChance, float pcriticalDamage)
    {
        damage = pdamage;
        statusChance = pstatusChance;
        criticalChance = pcriticalChance;
        criticalDamage = pcriticalDamage;

        transform.position = origin;
        transform.gameObject.SetActive(true);
        Vector2 direction = (target.position - origin).normalized;
        moveVector = speed * Time.fixedDeltaTime * direction;
        
        StartCoroutine(ProjectileMoving(duration, origin));
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVector);
    }

    IEnumerator ProjectileMoving(float duration, Vector3 origin)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            DamageStruct damageMessage = new()
            {
                damage = damage,
                criticalChance = criticalChance,
                criticalDamage = criticalDamage,
                statusChance = statusChance,
            };

            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damageMessage);

            Destroy(gameObject);
        }
    }
}
