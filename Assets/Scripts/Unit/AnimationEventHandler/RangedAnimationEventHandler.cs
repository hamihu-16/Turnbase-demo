using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject projectileProp;
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private Transform throwPoint;

    public static EventHandler OnAnyProjectileFired;
    public event EventHandler OnProjectileFired;
    public event EventHandler OnProjectileComplete;

    private Vector3 targetPosition;

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public void FireBullet()
    {
        Debug.Log("Projectile Fired");

        Transform projectileTransform = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);
        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        targetPosition.y = throwPoint.position.y;
        projectile.Setup(targetPosition);

        OnAnyProjectileFired?.Invoke(this, EventArgs.Empty);
        OnProjectileFired?.Invoke(this, EventArgs.Empty);
    }

    public void OnEquipProjectile()
    {
        projectileProp.SetActive(!projectileProp.activeInHierarchy);
    }

    public void ShootComplete()
    {
        OnProjectileComplete?.Invoke(this, EventArgs.Empty);
    }
}
