using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimationEventHandler : MonoBehaviour
{
    public static event EventHandler OnAnyMeleeHit;
    public event EventHandler OnMeleeHit;
    public event EventHandler OnMeleeComplete;

    private Vector3 targetPosition;

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public void SwordHit()
    {
        OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
        OnMeleeHit?.Invoke(this, EventArgs.Empty);
    }

    public void SwordComplete()
    {
        OnMeleeComplete?.Invoke(this, EventArgs.Empty);
    }
}
