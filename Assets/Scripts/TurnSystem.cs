using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnEnd;

    private int turnNumber = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two or more TurnSystem active!");
            Destroy(this.gameObject);
        }
        Instance = this;
    }
    public void NextTurn()
    {
        turnNumber++;
        OnTurnEnd?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return this.turnNumber;
    }
}
