using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private int totalCollectibles;
    [SerializeField] private int currentCollectibles;

    private void OnEnable()
    {
        GameSignals.OnCurrentCollectibles += HandleCollectibles;
        GameSignals.OnTotalCollectible += HandleTotal;
        GameSignals.OnGameOver += OnGameOver;
        
    }
    
    private void OnDisable()
    {
        GameSignals.OnTotalCollectible -= HandleTotal;
        GameSignals.OnCurrentCollectibles -= HandleCollectibles;
        GameSignals.OnGameOver -= OnGameOver;
    }
    
    void HandleCollectibles(int value) {
        currentCollectibles += value;

        if (currentCollectibles >= totalCollectibles)
        {
            GameSignals.TriggerGameOver(GameSignals.GameOver.Win);
        }
        
        GameSignals.TriggerOnSubmitCollectibles(currentCollectibles);
    }

    public void OnGameOver(GameSignals.GameOver value)
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
    }
    
    void HandleTotal(int value) {
        totalCollectibles = value;
    }
}
