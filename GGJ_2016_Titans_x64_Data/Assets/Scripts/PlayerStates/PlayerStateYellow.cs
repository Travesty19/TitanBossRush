using UnityEngine;
using System.Collections;

public class PlayerStateYellow : PlayerState {
    // State properties


    // State Methods
    public PlayerStateYellow()
    {
        m_ID = PlayerStates.Yellow;
    }


    public override void Enter()
    {
        Debug.Log("Enter " + GetStateName());
        PlayerController.Instance.SetColor(new Color(1.0f, 1.0f, 0.0f));
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