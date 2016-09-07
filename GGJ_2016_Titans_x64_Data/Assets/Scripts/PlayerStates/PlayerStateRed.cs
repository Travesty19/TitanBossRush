using UnityEngine;
using System.Collections;

public class PlayerStateRed : PlayerState {
    // State properties


    // State Methods
    public PlayerStateRed()
    {
        m_ID = PlayerStates.Red;
    }


    public override void Enter()
    {
        Debug.Log("Enter " + GetStateName());
        PlayerController.Instance.SetColor(new Color(1.0f, 0.0f, 0.0f));
    }


    public override void Exit()
    {
        Debug.Log("Exit " + GetStateName());
    }


    public override void UpdateState()
    {
    }


    public override bool Attack()
    {
        Debug.Log("Attack " + GetStateName());
        return true;
    }


    public override bool Defend()
    {
        Debug.Log("Defend " + GetStateName());
        return true;
    }
}
