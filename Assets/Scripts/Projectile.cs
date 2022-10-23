using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform projectileHitVFX;
    private Vector3 targetPosition;
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 shootDirection = (targetPosition - this.transform.position).normalized;
        float moveSpeed = 100f;

        float distanceStart = Vector3.Distance(targetPosition, this.transform.position);
        transform.position += shootDirection * moveSpeed * Time.deltaTime;
        float distanceEnd = Vector3.Distance(targetPosition, this.transform.position);

        if (distanceStart < distanceEnd)
        {
            // detach TrailRenderer from game object so that the trail doesnt end abrubtly 
            // when the bullet hit
            trailRenderer.transform.parent = null;

            // reset position to not overshoot
            transform.position = targetPosition;

            // instantiate bullet hit vfx
            Instantiate(projectileHitVFX, targetPosition, Quaternion.identity);

            // destroy bullet
            Destroy(gameObject);
        }
    }
}
