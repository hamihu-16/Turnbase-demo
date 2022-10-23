using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsUI;
    [SerializeField] private Image healthBar;
    [SerializeField] private Unit unit;
    [SerializeField] private HealthSystem healthSystem;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;    
    }

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnHealthChanged += HealthSystem_OnDamaged;
        UpdateActionPointsUI();
        UpdateHealthBar();
    }

    private void LateUpdate()
    {
        transform.forward = cameraTransform.forward;
    }

    private void UpdateActionPointsUI()
    {
        actionPointsUI.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = healthSystem.GetHealth() / healthSystem.getMaxHealth();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsUI();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
