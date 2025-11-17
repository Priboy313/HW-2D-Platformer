using System;
using UnityEngine;

static class DevRandom
{
    private static float s_chanceMax = 100f;

    public static int GetNumber(int min, int max)
    {
        return UnityEngine.Random.Range(Math.Min(min, max), Math.Max(min, max));
    }

    public static int GetNumber(int max)
    {
        return GetNumber(0, max);
    }

    public static float GetNumber(float min, float max)
    {
        return UnityEngine.Random.Range(Math.Min(min, max), Math.Max(min, max));
    }

    public static float GetNumber(float max)
    {
        return GetNumber(0f, max);
    }

    public static Vector3 GetVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(
            GetNumber(min.x, max.x),
            GetNumber(min.y, max.y),
            GetNumber(min.z, max.z)
        );
    }

    public static Vector3 GetVector3OnPlane()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized;

        return new Vector3(
            randomCircle.x,
            0,
            randomCircle.y
        );
    }

    public static bool IsChanceSuccess(int chancePercent)
    {
        return UnityEngine.Random.value < (chancePercent / s_chanceMax);
    }

    public static bool IsChanceSuccess(float chancePercent)
    {
        return UnityEngine.Random.value < (chancePercent / s_chanceMax);
    }
}
