using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private static int StartValue = 25;

    public static int MaxValue { get; } = 100;

    public static int Value { get; private set; } = 25;

    public static event Action ValueChanged;

    private void Start()
    {
        StartValue = MaxValue / 4;

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
