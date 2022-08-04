using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateActionState : State
{
    [SerializeField] private SearchTargetState searchTargetState;
    [SerializeField] private BusyState busyState;

    protected override void Awake()
    {
        
    }

    protected override State Tick()
    {
        return this;
    }

}
