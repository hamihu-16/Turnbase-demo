using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected abstract void Awake();
    protected abstract State Tick();
}