using UnityEngine;

public class SpawnPoint : MonoBehaviour 
{
    public Vector3 SpawnPosition => transform.position;
    public bool IsFree { get; private set; } = true;

    public void SetFree(bool isFree)
    {
        IsFree = isFree;
    }
}