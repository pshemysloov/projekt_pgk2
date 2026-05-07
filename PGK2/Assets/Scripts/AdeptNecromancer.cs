using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdeptNecromancer : EnemyAI
{
    public Projectile projectilePrefab;
    private Vector3 projectileOrigin;


    [SerializeField]
    private GameEvent EnemyHealthChanged;


    public override IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                //Attack logic
                movementInput = Vector2.zero;
                projectileOrigin = transform.position;
                projectileOrigin.x += projectileOffset.x;
                projectileOrigin.y += projectileOffset.y;

                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //Chase logic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }

        }

    }

    public override int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            float completionRatio = (float)currentHealth / (float)maxHealth;
            EnemyHealthChanged.Raise(this, completionRatio);
        }
    }

    public override void Attack()
    {
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.Attack(aiData.currentTarget, projectileOrigin, projectileSpeed, projectileDuration, damage, statusChance, criticalChance, criticalDamage);
    }
}
