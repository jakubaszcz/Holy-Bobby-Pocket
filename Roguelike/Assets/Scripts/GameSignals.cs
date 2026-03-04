using System;
using UnityEngine;

public static class GameSignals
{
    public static event Action<float> OnSeenValueChanged;
    public static event Action<bool> IsSeenChanged;
	public static event Action<int> OnTotalCollectible;
    
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
}