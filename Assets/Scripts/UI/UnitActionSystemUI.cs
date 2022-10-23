using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    // add event to change remaining action point here

    [SerializeField] private Transform unitActionButtonUIPrefab;
    [SerializeField] private Transform unitActionButtonContainer;
    [SerializeField] private Transform unitActionBusyUI;
    [SerializeField] private Transform actionPointsUI;

    private List<UnitActionButtonUI> unitActionButtonUIList;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        TurnSystem.Instance.OnTurnEnd += TurnSystem_OnTurnEnd;

        unitActionButtonUIList = new List<UnitActionButtonUI>();
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform unitActionButtonTransfrom = Instantiate(unitActionButtonUIPrefab, unitActionButtonContainer);
            UnitActionButtonUI unitActionButtonUI = unitActionButtonTransfrom.GetComponent<UnitActionButtonUI>();
            unitActionButtonUI.SetBaseAction(baseAction);
            unitActionButtonUIList.Add(unitActionButtonUI);
        }
    }

    private void ClearUnitActionButtons()
    {
        unitActionButtonUIList.Clear();
        foreach(Transform unitActionButtonTransform in unitActionButtonContainer)
        {
            Destroy(unitActionButtonTransform.gameObject);
        }
    }

    public void UpdateSelectedButtonVisual()
    {
        foreach (UnitActionButtonUI unitActionButtonUI in unitActionButtonUIList)
        {
            unitActionButtonUI.UpdateSelectedButtonVisual();
        }
    }

    private void HandleActionPointUI()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit != null)
        {
            actionPointsUI.gameObject.SetActive(true);
            actionPointsUI.GetComponent<TextMeshProUGUI>().text = "Action Points Remaining: " + selectedUnit.GetActionPoints();
        }
        else
        {
            actionPointsUI.gameObject.SetActive(false);
        }        
    }


    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        ClearUnitActionButtons();
        CreateUnitActionButtons();
        UpdateSelectedButtonVisual();
        HandleActionPointUI();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedButtonVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        HandleActionPointUI();
    }

    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        HandleActionPointUI();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        HandleActionPointUI();
    }

}
