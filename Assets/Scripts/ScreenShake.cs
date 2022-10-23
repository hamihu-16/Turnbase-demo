using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SearchService;
using System;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two or more ScreenShake Instance active");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        Debug.Log(cinemachineImpulseSource);
        RangedAnimationEventHandler.OnAnyProjectileFired += rangedAnimationEventHandler_OnAnyProjectileFired;
        MeleeAnimationEventHandler.OnAnyMeleeHit += meleeAnimationEventHandler_OnAnyMeleeHit;
    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }

    private void rangedAnimationEventHandler_OnAnyProjectileFired(object sender, EventArgs e) 
    {
        Shake(1f);
    }

    private void meleeAnimationEventHandler_OnAnyMeleeHit(object sender, EventArgs e)
    {
        Shake(0.75f);
    }
}
