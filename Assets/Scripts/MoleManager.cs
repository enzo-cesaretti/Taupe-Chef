using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MoleManager : MonoBehaviour
{
    public static MoleManager Instance { get; private set; }

    public float SpawnRateMultiplier => _spawnRateMultiplier;
    public float LuckMultiplier => _luckMultiplier;

    [Header("Moles")]
    [SerializeField] private List<Mole> moles = new();

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDelay = 3f;
    [SerializeField] private float maxSpawnDelay = 6f;
    [SerializeField] private int maxMolesUp = 4;
    [SerializeField] private float _spawnRateMultiplier = 1f;
    [SerializeField] private float _luckMultiplier = 1f;

    [Header("Stay Up Time")]
    [SerializeField] private float minStayDuration = 1.5f;
    [SerializeField] private float maxStayDuration = 3f;

    private bool isRunning = false;
    private Coroutine loopRoutine;

    // Track active timers per mole
    private readonly Dictionary<Mole, Coroutine> activeMoleTimers = new();

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


    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (isRunning) return;

        isRunning = true;
        loopRoutine = StartCoroutine(SpawnLoop());
    }

    public void PauseGame()
    {
        isRunning = false;

        if (loopRoutine != null)
            StopCoroutine(loopRoutine);

        foreach (var timer in activeMoleTimers)
        {
            if (timer.Value != null)
                StopCoroutine(timer.Value);
        }

        activeMoleTimers.Clear();
    }

    public void ResetGame()
    {
        PauseGame();

        foreach (var mole in moles)
        {
            if (mole.State != MoleState.Down)
                mole.MoleDown();
        }
    }

    public void SetSpawnRateMultiplier(float multiplier)
    {
        _spawnRateMultiplier = multiplier;
    }

    public void SetLuckMultiplier(float multiplier)
    {
        _luckMultiplier = multiplier;
    }

    private IEnumerator SpawnLoop()
    {
        while (isRunning)
        {
            var delay = Random.Range(minSpawnDelay - (0.5f * _spawnRateMultiplier), maxSpawnDelay - (0.5f * _spawnRateMultiplier));

            yield return new WaitForSeconds(delay);

            TrySpawnMole();
        }
    }

    private void TrySpawnMole()
    {
        if (GetActiveMoleCount() >= maxMolesUp)
            return;

        Mole mole = GetRandomDownMole();

        if (mole != null)
        {
            CancelMoleTimer(mole);

            var goldenChance = 0.1f * _luckMultiplier;
            var isGolden = Random.value < goldenChance;

            mole.MoleUp(isGolden);

            float stayTime = Random.Range(minStayDuration, maxStayDuration);

            Coroutine timer = StartCoroutine(MoleStayRoutine(mole, stayTime));

            activeMoleTimers[mole] = timer;
        }
    }

    public void CancelMoleTimer(Mole mole)
    {
        if (activeMoleTimers.TryGetValue(mole, out Coroutine routine))
        {
            if (routine != null)
                StopCoroutine(routine);

            activeMoleTimers.Remove(mole);
        }
    }

    private IEnumerator MoleStayRoutine(Mole mole, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (mole != null && mole.State == MoleState.Up)
        {
            mole.MoleDown();
        }

        activeMoleTimers.Remove(mole);
    }

    private int GetActiveMoleCount()
    {
        int count = 0;

        foreach (var mole in moles)
        {
            if (mole.State != MoleState.Down)
                count++;
        }

        return count;
    }

    private Mole GetRandomDownMole()
    {
        List<Mole> available = new();

        foreach (var mole in moles)
        {
            if (mole.State == MoleState.Down)
                available.Add(mole);
        }

        if (available.Count == 0)
            return null;

        return available[Random.Range(0, available.Count)];
    }
}