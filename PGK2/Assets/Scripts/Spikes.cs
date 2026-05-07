using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Player player;
    public float damage;
    private DamageStruct damageStr;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            damageStr.damage = damage;
            player = collider.gameObject.GetComponent<Player>();
            if (player.isDashing == false)
            {
                player.TakeDamage(damageStr);
            }
        }
    }
}
