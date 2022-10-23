using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawn : MonoBehaviour
{
    private HealthSystem healthSystem;
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform unitRootBone;


    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, Quaternion.identity);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(unitRootBone);
        
    }
}
