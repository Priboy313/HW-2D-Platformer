using System;
public interface IInput
{
    public event Action<float> Moving;
    public event Action Jumped;
}
