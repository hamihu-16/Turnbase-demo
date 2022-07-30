using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform unitActionButtonUIPrefab;
    [SerializeField] private Transform unitActionButtonContainer;
    [SerializeField] private Transform unitActionBusyUI;

    private List<UnitActionButtonUI> unitActionButtonUIList;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
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

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        ClearUnitActionButtons();
        CreateUnitActionButtons();
        UpdateSelectedButtonVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedButtonVisual();
    }

    private void UnitActionSystem_OnBusyChanged(object sender, EventArgs e)
    {
        Debug.Log("UnitActionSystem_OnBusyChanged" + UnitActionSystem.Instance.GetBusy());
        unitActionBusyUI.GetComponent<UnitActionBusyUI>().UpdateActionBusy();
    }
}
