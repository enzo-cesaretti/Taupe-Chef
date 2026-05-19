using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField]
    private List<ShopItemData> _shopItems;

    private readonly Dictionary<ShopItemType, int> _purchasedTiers = new();

    public event Action<ShopItemType, int>? OnItemPurchased;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Initialize();
    }

    private void Initialize()
    {
        foreach (var item in _shopItems)
        {
            _purchasedTiers[item.ItemType] = 0;
        }
    }

    public int GetCurrentTier(ShopItemType type)
    {
        return _purchasedTiers[type];
    }

    public int GetUpgradeCost(ShopItemType type)
    {
        ShopItemData item = GetItemData(type);

        int currentTier = _purchasedTiers[type];

        return Mathf.RoundToInt(
            item.BaseCost *
            Mathf.Pow(item.CostMultiplier, currentTier));
    }

    public float GetUpgradeValue(ShopItemType type)
    {
        ShopItemData item = GetItemData(type);

        int currentTier = _purchasedTiers[type];

        return
            item.BaseValue *
            Mathf.Pow(item.ValueMultiplier, currentTier);
    }
    public bool TryPurchase(ShopItemType type)
    {
        ShopItemData item = GetItemData(type);

        int currentTier = _purchasedTiers[type];

        if (currentTier >= item.MaxTier)
        {
            Debug.Log("Max tier reached");
            return false;
        }

        int cost = GetUpgradeCost(type);

        if (!CoinManager.Instance.HasEnoughCoins(cost))
        {
            Debug.Log("Not enough coins");
            return false;
        }

        CoinManager.Instance.RemoveCoins(cost);

        _purchasedTiers[type]++;

        float value = GetUpgradeValue(type);

        ApplyUpgrade(type, value);

        OnItemPurchased?.Invoke(type, currentTier + 1);

        return true;
    }

    private void ApplyUpgrade(ShopItemType type, float value)
    {
        switch (type)
        {
            case ShopItemType.CoinMultiplier:
                CoinManager.Instance.SetMultiplier(value);
                Debug.Log($"Coin multiplier upgraded to {value}");
                break;

            case ShopItemType.LuckMultiplier:
                Debug.Log($"Luck multiplier upgraded to {value}");
                break;

            case ShopItemType.SpawnRateMultiplier:
                Debug.Log($"Spawn rate multiplier upgraded to {value}");
                break;

            default:
                Debug.LogWarning($"No upgrade logic defined for {type}");
                break;
        }
    }

    public ShopItemData GetItemData(ShopItemType type)
    {
        foreach (var item in _shopItems)
        {
            if (item.ItemType == type)
            {
                return item;
            }
        }

        throw new Exception($"Missing ShopItemData for {type}");
    }
}