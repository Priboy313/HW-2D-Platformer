using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out Coin coin))
        {
            coin.Collect();
        }
    }
}
