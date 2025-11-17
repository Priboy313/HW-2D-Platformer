using UnityEngine;

public class InGameAutoDeactivate : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
