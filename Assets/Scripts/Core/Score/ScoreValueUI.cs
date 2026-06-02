using TMPro;
using UnityEngine;

public sealed class ScoreValueUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager not found.");
            enabled = false;
            return;
        }

        UpdateScoreText(ScoreManager.Instance.CurrentScore);

        ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
        }
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString("N0");
    }
}