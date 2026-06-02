using System;
using UnityEngine;

public sealed class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public int CurrentCoins => _currentCoins;
    public float CoinMultiplier => _coinMultiplier;

    public event Action<int>? OnCoinsChanged;
    public event Action<float>? OnMultiplierChanged;

    [SerializeField]
    private int _currentCoins;

    [SerializeField]
    private float _coinMultiplier = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void AddCoins(int baseAmount)
    {
        if (baseAmount <= 0)
        {
            Debug.LogWarning($"Trying to add invalid coin amount: {baseAmount}");
            return;
        }

        int finalAmount = Mathf.RoundToInt(baseAmount * _coinMultiplier);

        _currentCoins += finalAmount;

        OnCoinsChanged?.Invoke(_currentCoins);
    }

    public bool RemoveCoins(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Trying to remove invalid coin amount: {amount}");
            return false;
        }

        if (_currentCoins < amount)
        {
            return false;
        }

        _currentCoins -= amount;

        OnCoinsChanged?.Invoke(_currentCoins);

        return true;
    }

    public void SetMultiplier(float multiplier)
    {
        if (multiplier < 0f)
        {
            Debug.LogWarning($"Invalid multiplier: {multiplier}");
            return;
        }

        _coinMultiplier = multiplier;

        OnMultiplierChanged?.Invoke(_coinMultiplier);
    }

    public void ResetCoins()
    {
        _currentCoins = 0;

        OnCoinsChanged?.Invoke(_currentCoins);
    }

    public bool HasEnoughCoins(int amount)
    {
        return _currentCoins >= amount;
    }
}