using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueCrow : EnemyAI
{
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
                animator.SetBool("isMoving", false);

                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //Chase logic
                animator.SetBool("isMoving", true);
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }

        }

    }

    public override void Attack()
    {
        Vector2 playerPosVector = (Vector2)(aiData.currentTarget.position - transform.position);
        if (playerPosVector.y + 0.4 > 0)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("JumpAttack");
        }

    }

    public override void Movement()
    {
        base.Movement();

        if (movementInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (movementInput.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
        }
        else if (movementInput.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
        }

    }

}
