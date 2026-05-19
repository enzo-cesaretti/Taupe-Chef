using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField]
    private ShopItemType _itemType;

    public void Purchase()
    {
        ShopManager.Instance.TryPurchase(_itemType);
    }
}