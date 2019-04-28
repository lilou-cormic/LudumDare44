using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private static int StartValue = 50;

    public static int MaxValue { get; } = 200;

    public static int Value { get; private set; } = 50;

    public static event Action ValueChanged;

    private void Start()
    {
        Value = StartValue;

        ValueChanged?.Invoke();
    }

    public static void AddHealth(int value)
    {
        Value = Mathf.Min(Value + value, MaxValue);

        ValueChanged?.Invoke();
    }

    public static void RemoveHealth(int value)
    {
        Value = Mathf.Max(Value - value, 0);

        ValueChanged?.Invoke();
    }
}
