using UnityEngine;
using System.Collections;

public abstract class PlayerState {
    // Properties
    public enum PlayerStates { None, Red, Green, Blue, Yellow }
    protected PlayerStates m_ID = PlayerStates.None;


    // Methods
    public PlayerStates GetStateID()
    {
        return m_ID;
    }

    public string GetStateName()
    {
        return this.ToString();
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateState();
    public abstract bool Attack();
    public abstract bool Defend();
}