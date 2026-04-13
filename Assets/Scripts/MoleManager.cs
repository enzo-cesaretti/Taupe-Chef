using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleManager : MonoBehaviour
{
    [Header("Moles")]
    [SerializeField] private List<Mole> moles = new();

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float maxSpawnDelay = 1.5f;
    [SerializeField] private int maxMolesUp = 4;

    [Header("Stay Up Time")]
    [SerializeField] private float minStayDuration = 1.5f;
    [SerializeField] private float maxStayDuration = 3f;

    private bool isRunning = false;
    private Coroutine loopRoutine;

    // Track active timers per mole
    private Dictionary<Mole, Coroutine> activeMoleTimers = new();

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
            if (mole.State != Mole.MoleState.Down)
                mole.MoleDown();
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

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
            mole.MoleUp();

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

        if (mole != null && mole.State == Mole.MoleState.Up)
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
            if (mole.State != Mole.MoleState.Down)
                count++;
        }

        return count;
    }

    private Mole GetRandomDownMole()
    {
        List<Mole> available = new();

        foreach (var mole in moles)
        {
            if (mole.State == Mole.MoleState.Down)
                available.Add(mole);
        }

        if (available.Count == 0)
            return null;

        return available[Random.Range(0, available.Count)];
    }
}