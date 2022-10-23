using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 15f;
    private float throwDistance;
    private Vector3 positionXZ;
    private Action onGrenadeBehaviourComplete;

    [SerializeField] private Transform grenadeExplosionVFX;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    public event EventHandler OnAnyGrenadeExploded;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;

        positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / throwDistance;

        float maxHeight = throwDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);


        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.TakeDamage(30);
                }
                if (collider.TryGetComponent<DestructibleObject>(out DestructibleObject destructibleObject))
                {
                    destructibleObject.TakeDamage();
                }
            }

            Instantiate(grenadeExplosionVFX, targetPosition + Vector3.up, Quaternion.identity);
            trailRenderer.transform.parent = null;
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            onGrenadeBehaviourComplete();
        }
    }


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPositionInLevelGrid(targetGridPosition);
        positionXZ = transform.position;
        positionXZ.y = 0;
        throwDistance = Vector3.Distance(positionXZ, targetPosition);

    }

}
