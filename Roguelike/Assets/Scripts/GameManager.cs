using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private int totalCollectibles;

    private void OnEnable()
    {
        GameSignals.OnTotalCollectible += HandleTotal;
    }
    
    private void OnDisable()
    {
        GameSignals.OnTotalCollectible -= HandleTotal;
    }
    
    void HandleTotal(int value) {
        totalCollectibles = value;
    }
}
