using System;
using UnityEngine;

public static class GameSignals
{

    public enum GameOver
    {
        Win,
        Lose
    }

    public enum Rank
    {
        D,
        C,
        B,
        A,
        S
    }
    
    public static event Action<float> OnSeenValueChanged;
    
    public static event Action<Rank> OnRank;
    public static event Action<bool> IsSeenChanged;
	public static event Action<int> OnTotalCollectible;
    public static event Action<int> OnCurrentCollectibles;
    public static event Action<int> OnSubmitCurrentCollectibles;
    public static event Action<float> OnCollectingCollectible;

    public static event Action<bool> IsInRange;

    public static event Action<bool> OnStartTimer;
    public static event Action<int> OnTimer;
    public static event Action<int> OnEndGameTimer;

    public static event Action<int> OnSpot;
    public static event Action<bool> OnEndGame;
    
    public static event Action<GameOver> OnGameOver;

    public static event Action<bool> OnTrapped;
    
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

    public static void TriggerOnTotalCollectible(int value) { OnTotalCollectible?.Invoke(value); } 
    public static void TriggerOnStartTimer(bool value) { OnStartTimer?.Invoke(value); } 
	public static void TriggerOnCurrentCollectibles(int value) { OnCurrentCollectibles?.Invoke(value); } 
    public static void TriggerOnSubmitCollectibles(int value) { OnSubmitCurrentCollectibles?.Invoke(value); } 
	public static void TriggerOnSpot(int value) { OnSpot?.Invoke(value); } 
	public static void TriggerOnTimer(int value) { OnTimer?.Invoke(value); } 
    
    public static void TriggerIsInRange(bool value) { IsInRange?.Invoke(value); }
    
    public static void TriggerOnCollectingCollectible(float value) { OnCollectingCollectible?.Invoke(value); }
    public static void TriggerOnEndGame(bool value) { OnEndGame?.Invoke(value); }
    
    public static void TriggerEndGameTimer(int value) { OnEndGameTimer?.Invoke(value); }
    
    public static void TriggerRank(Rank value) { OnRank?.Invoke(value); }
    public static void TriggerTrapped(bool value) { OnTrapped?.Invoke(value); }
    public static void TriggerGameOver(GameOver gameOver) { OnGameOver?.Invoke(gameOver); }
}