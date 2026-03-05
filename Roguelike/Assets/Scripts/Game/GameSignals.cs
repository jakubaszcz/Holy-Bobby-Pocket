using System;
using UnityEngine;

public static class GameSignals
{

    public enum GameOver
    {
        Win,
        Lose
    }
    
    public static event Action<float> OnSeenValueChanged;
    public static event Action<bool> IsSeenChanged;
	public static event Action<int> OnTotalCollectible;
    public static event Action<int> OnCurrentCollectibles;
    public static event Action<int> OnSubmitCurrentCollectibles;

    public static event Action<bool> OnStartTimer;
    public static event Action<int> OnTimer;

    public static event Action<int> OnSpot;
    
    public static event Action<GameOver> OnGameOver;
    
    private static int enemiesSeeingPlayer = 0;

    public static void ResetEnemyCounter()
    {
        enemiesSeeingPlayer = 0;
        IsSeenChanged?.Invoke(false);
    }

    public static void SetEnemySeeing(bool isSeen)
    {
        enemiesSeeingPlayer += isSeen ? 1 : -1;
        enemiesSeeingPlayer = Mathf.Max(0, enemiesSeeingPlayer);
        IsSeenChanged?.Invoke(enemiesSeeingPlayer > 0);
    }
    
    public static void TriggerSeenValueChanged(float value) { OnSeenValueChanged?.Invoke(value); }

    public static void TriggerOnTotalColectible(int value) { OnTotalCollectible?.Invoke(value); } 
    public static void TriggerOnStartTimer(bool value) { OnStartTimer?.Invoke(value); } 
	public static void TriggerOnCurrentCollectibles(int value) { OnCurrentCollectibles?.Invoke(value); } 
    public static void TriggerOnSubmitCollectibles(int value) { OnSubmitCurrentCollectibles?.Invoke(value); } 
	public static void TriggerOnSpot(int value) { OnSpot?.Invoke(value); } 
	public static void TriggerOnTimer(int value) { OnTimer?.Invoke(value); } 
    
    
    public static void TriggerGameOver(GameOver gameOver) { OnGameOver?.Invoke(gameOver); }
}