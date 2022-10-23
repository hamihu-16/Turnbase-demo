using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> playerUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two or more UnitManager active!");
            Destroy(this);
        }
        Instance = this;
        
        unitList = new List<Unit>();
        playerUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Add(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            playerUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            playerUnitList.Remove(unit);
        }
    }

    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        Debug.Log("TurnEnd");
    }

    public List<Unit> GetUnitList()
    {
        return this.unitList;
    }
    public List<Unit> GetPlayerUnitList()
    {
        return this.playerUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return this.enemyUnitList;
    }
}
