using System;
using UnityEngine;

public class InputReader : MonoBehaviour 
{
    public event Action<float> ActionMove;
    public event Action ActionJump;

    private void Update()
    {
        ActionMove?.Invoke(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButtonDown("Jump"))
        {
            ActionJump?.Invoke();
        }
    }
}