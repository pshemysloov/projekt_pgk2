using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPushAttack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isDashing", true);
        GameManager.instance.player.isWeaponDash = true;
        GameManager.instance.player.OnDash();
    }
}
