using System;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [Header("Player statistics")]
    [SerializeField] private float maxTimeSeen = 3f;
    [SerializeField] private float currentSeen = 0f;
    [SerializeField] private bool isSeen;

    private void Start()
    {
        isSeen = false;
        GameSignals.ResetEnemyCounter();
    }
    
    private void Update()
    {
        if (isSeen)
        {
            currentSeen += Time.deltaTime;
            
            if (currentSeen >= maxTimeSeen)
            {
                currentSeen = maxTimeSeen;
                GameSignals.TriggerGameOver(GameSignals.GameOver.Lose);
            }
        }
        else
        {
            if (currentSeen > 0f)
            {
                currentSeen -= Time.deltaTime;
                if (currentSeen < 0f)
                {
                    currentSeen = 0f;
                }
            }
        }
        
        Debug.Log(currentSeen);
        
        GameSignals.TriggerSeenValueChanged(currentSeen / maxTimeSeen);
    }

    private void IsSeenChanged(bool value)
    {
        isSeen = value;
        Debug.Log("Is seen :" + isSeen);
    }
    
    private void OnEnable()
    {
        GameSignals.IsSeenChanged += IsSeenChanged; 
    }

    private void OnDisable()
    {
        GameSignals.IsSeenChanged -= IsSeenChanged;

    }
}
