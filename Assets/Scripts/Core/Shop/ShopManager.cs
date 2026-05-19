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

    public bool TryPurchase(ShopItemType type)
    {
        ShopItemData item = GetItemData(type);

        int currentTier = _purchasedTiers[type];

        if (currentTier >= item.Tiers.Count)
        {
            Debug.Log("Max tier reached");
            return false;
        }

        ShopTier nextTier = item.Tiers[currentTier];

        if (!CoinManager.Instance.HasEnoughCoins(nextTier.Cost))
        {
            Debug.Log("Not enough coins");
            return false;
        }

        CoinManager.Instance.RemoveCoins(nextTier.Cost);

        _purchasedTiers[type]++;

        ApplyUpgrade(type, nextTier);

        OnItemPurchased?.Invoke(type, nextTier.Tier);

        Debug.Log($"Purchased {type} Tier {nextTier.Tier}");

        return true;
    }

    private void ApplyUpgrade(ShopItemType type, ShopTier tier)
    {
        switch (type)
        {
            case ShopItemType.CoinMultiplier:
                CoinManager.Instance.SetMultiplier(tier.Value);
                break;

            case ShopItemType.LuckMultiplier:
                Debug.Log($"Luck multiplier upgraded to {tier.Value}");
                break;

            case ShopItemType.SpawnRateMultiplier:
                Debug.Log("Golden spatula unlocked");
                break;

            default:
                Debug.LogWarning($"No upgrade logic defined for {type}");
                break;
        }
    }

    private ShopItemData GetItemData(ShopItemType type)
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