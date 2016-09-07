using UnityEngine;
using System.Collections;

public class PlayerAnimatorTeleport : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Teleporting", false);
    }
}