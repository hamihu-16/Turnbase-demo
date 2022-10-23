using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Transform projectilePrefab;

    public static EventHandler OnAnyBuffFired;
    public event EventHandler OnBuffFired;
    public event EventHandler OnBuffComplete;

    private Vector3 targetPosition;

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public void UseBuff()
    {
        Debug.Log("Buff Used");

        Transform projectileTransform = Instantiate(projectilePrefab, targetPosition, Quaternion.identity);

        OnAnyBuffFired?.Invoke(this, EventArgs.Empty);
        OnBuffFired?.Invoke(this, EventArgs.Empty);
    }

    public void BuffComplete()
    {
        OnBuffComplete?.Invoke(this, EventArgs.Empty);

    }
}
