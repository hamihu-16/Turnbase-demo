using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] private SearchTargetState searchTargetState;

    protected override void Awake()
    {
        
    }

    protected override State Tick()
    {
        // Wait for player's turn to end
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return this;
        }
        else
        {
            return searchTargetState;
        }
    }
}
