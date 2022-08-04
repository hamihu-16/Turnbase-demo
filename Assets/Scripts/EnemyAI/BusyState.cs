using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusyState : State
{

    protected override void Awake()
    {
        throw new System.NotImplementedException();
    }

    protected override State Tick()
    {
        return this;
    }

    
}
