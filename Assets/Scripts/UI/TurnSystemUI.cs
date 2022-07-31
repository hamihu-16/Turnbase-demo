using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    private Button endTurnButton;
    private TextMeshProUGUI turnTextMeshPro;

    private void Awake()
    {
        endTurnButton = GetComponentInChildren<Button>();
        turnTextMeshPro = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
            
        });

        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;

        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnTextMeshPro.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        UpdateTurnText();
    }
}
