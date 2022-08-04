using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    private Animator unitAnimator;

    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform gunTip;

    private void Awake()
    {
        unitAnimator = GetComponentInChildren<Animator>();
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        unitAnimator.SetBool("IsMoving", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        unitAnimator.SetBool("IsMoving", false);
    }

    private void ShootAction_OnShoot(object sender, EventArgs e)
    {
        unitAnimator.SetTrigger("Shoot");

        Instantiate(bulletProjectilePrefab, gunTip.position, Quaternion.identity);
    }
}
