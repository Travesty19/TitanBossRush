﻿using UnityEngine;
using System.Collections;

public class PlayerAnimatorDefend : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Defending", false);
    }
}