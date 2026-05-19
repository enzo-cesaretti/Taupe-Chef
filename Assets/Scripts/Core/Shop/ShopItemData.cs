using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TaupeChef/Shop Item")]
public class ShopItemData : ScriptableObject
{
    public ShopItemType ItemType;

    public List<ShopTier> Tiers = new();
}