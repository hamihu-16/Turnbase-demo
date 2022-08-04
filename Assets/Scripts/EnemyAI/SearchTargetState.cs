using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTargetState : State
{
    [SerializeField] private CalculateActionState calculateActionState;
    private List<Unit> playerUnitList;

    protected override void Awake()
    {
        //playerUnit = UnitManager.Instance.GetPlayerUnitList();
    }

    protected override State Tick()
    {
        // look for the closest target with lowest health from player unit list
        foreach (Unit playerUnit in playerUnitList)
        {
            // calculate distance based on pathfinding
            // return the playerUnit || pass it onto the next state
            // if not found then return this
        }
        return calculateActionState;
    }

    
}
