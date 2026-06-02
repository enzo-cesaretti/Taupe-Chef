using TMPro;
using UnityEngine;

public sealed class CoinValueUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;

    private void Start()
    {
        if (CoinManager.Instance == null)
        {
            Debug.LogError("CoinManager not found.");
            enabled = false;
            return;
        }

        UpdateCoinText(CoinManager.Instance.CurrentCoins);

        CoinManager.Instance.OnCoinsChanged += UpdateCoinText;
    }

    private void OnDestroy()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinsChanged -= UpdateCoinText;
        }
    }

    private void UpdateCoinText(int coins)
    {
        coinText.text = coins.ToString();
    }
}