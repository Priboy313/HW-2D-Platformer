using System;
public interface IInput
{
    public event Action<float> ActionMove;
    public event Action ActionJump;
}
