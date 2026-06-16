using UnityEngine;

[CreateAssetMenu(menuName = "TaupeChef/Shop Item")]
public class ShopItemData : ScriptableObject
{
    public ShopItemType ItemType;

    [Header("Cost")]
    public int BaseCost = 100;
    public float CostMultiplier = 2f;

    [Header("Value")]
    public float BaseValue = 1f;
    public float ValueMultiplier = 1.5f;

    [Header("Tier")]
    public int MaxTier = 5;
}