using System;
using UnityEngine;

public interface IInput
{
    public event Action<float> Moving;
    public event Action Jumped;
    public event Action<KeyCode> AbilityKeyPressed;
}
