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
    }
    
    private void OnDisable()
    {
        GameSignals.OnTotalCollectible -= HandleTotal;
        GameSignals.OnCurrentCollectibles -= HandleCollectibles;
    }
    
    void HandleCollectibles(int value) {
        currentCollectibles += value;
        GameSignals.TriggerOnSubmitCollectibles(currentCollectibles);
    }
    
    void HandleTotal(int value) {
        totalCollectibles = value;
    }
}
