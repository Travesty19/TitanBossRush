using UnityEngine;
using System.Collections;

public class PlayerAnimatorAttack : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", false);
    }
}