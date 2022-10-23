using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator unitAnimator;
    private MeleeAnimationEventHandler meleeAnimationEventHandler;
    private RangedAnimationEventHandler rangedAnimationEventHandler;
    private BuffAnimationEventHandler buffAnimationEventHandler;

    private void Awake()
    {
        unitAnimator = GetComponentInChildren<Animator>();
        meleeAnimationEventHandler = GetComponentInChildren<MeleeAnimationEventHandler>();
        rangedAnimationEventHandler = GetComponentInChildren<RangedAnimationEventHandler>();
        buffAnimationEventHandler = GetComponentInChildren<BuffAnimationEventHandler>();

        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot+= ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSword += SwordAction_OnSword;
        }

        if (TryGetComponent<HealAction>(out HealAction healAction))
        {
            healAction.OnHeal += HealAction_OnHeal;
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

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        unitAnimator.SetTrigger("Shoot");
        rangedAnimationEventHandler.SetTargetPosition(e.targetUnit.GetWorldPosition());
    }

    private void SwordAction_OnSword(object sender, SwordAction.OnSwordEventArgs e)
    {
        unitAnimator.SetTrigger("Sword");
        meleeAnimationEventHandler.SetTargetPosition(e.targetUnit.GetWorldPosition());
    }
    private void HealAction_OnHeal(object sender, HealAction.OnHealEventArgs e)
    {
        unitAnimator.SetTrigger("Heal");
        buffAnimationEventHandler.SetTargetPosition(e.targetUnit.GetWorldPosition());
    }
}
