using TMPro;
using UnityEngine;

public class ShopTierUI : MonoBehaviour
{
    [SerializeField]
    private ShopItemType _itemType;

    [SerializeField]
    private TMP_Text _tierText;

    private void Start()
    {
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.OnItemPurchased += OnItemPurchased;
        }

        Refresh();
    }

    private void OnDestroy()
    {
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.OnItemPurchased -= OnItemPurchased;
        }
    }

    private void OnItemPurchased(
        ShopItemType itemType,
        int tier)
    {
        if (itemType != _itemType)
        {
            return;
        }

        Refresh();
    }

    public void Refresh()
    {
        if (ShopManager.Instance == null)
        {
            return;
        }

        int currentTier =
            ShopManager.Instance.GetCurrentTier(_itemType);

        ShopItemData itemData =
            ShopManager.Instance.GetItemData(_itemType);

        _tierText.text =
            $"Tier {currentTier}/{itemData.MaxTier}";
    }
}