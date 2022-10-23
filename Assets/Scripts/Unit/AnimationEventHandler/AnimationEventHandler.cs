using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject gunProp;
    [SerializeField] private GameObject gunShoot;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform gunTip;

    public static EventHandler OnAnyBulletFired;
    public event EventHandler OnBulletFired;
    public event EventHandler OnShootComplete;

    private Vector3 targetPosition;
    
    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public void FireBullet()
    {
        Debug.Log("Bullet Fired");  

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, gunTip.position, Quaternion.identity);
        Projectile bulletProjectile = bulletProjectileTransform.GetComponent<Projectile>();
        targetPosition.y = gunTip.position.y;
        bulletProjectile.Setup(targetPosition);

        OnAnyBulletFired?.Invoke(this, EventArgs.Empty);
        OnBulletFired?.Invoke(this, EventArgs.Empty);
    }

    public void OnEquipGun()
    {
        gunProp.SetActive(!gunProp.activeInHierarchy);
        gunShoot.SetActive(!gunShoot.activeInHierarchy);
    }

    public void ShootComplete()
    {
        OnShootComplete?.Invoke(this, EventArgs.Empty);
    }
}
