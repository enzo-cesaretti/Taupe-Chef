using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnScoreAdded;
    public event Action<int> OnScoreSpent;

    [SerializeField]
    private int currentScore = 0;

    public int CurrentScore => currentScore;

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

    public void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
        OnScoreAdded?.Invoke(amount);
        print(currentScore);
    }

    public bool SpendScore(int amount)
    {
        if (amount <= 0)
            return false;
        if (currentScore < amount)
            return false;

        currentScore -= amount;

        OnScoreChanged?.Invoke(currentScore);
        OnScoreSpent?.Invoke(amount);

        return true;
    }

    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }
}