using UnityEngine;
using TMPro;

public class InterfaceText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TMP_Text textObjectives;

    [SerializeField]
    private int maxCollectibles;

    private void Start()
    {
        textObjectives.text = "Collect all the collectibles 0/0";
    }

    private void TotalCollectibles(int value)
    {
        textObjectives.text = "Collect all the collectibles 0/" + value;
        
        maxCollectibles = value;
    }

    private void CurrentCollectibles(int value)
    {
        textObjectives.text = "Collect all the collectibles " + value + "/" + maxCollectibles;
    }
    
    private void DestroyAll(GameSignals.GameOver gameOverSignal)
    {
        Destroy(textObjectives);
    }
    private void OnEnable()
    {
        GameSignals.OnGameOver += DestroyAll;
        GameSignals.OnTotalCollectible += TotalCollectibles;
        GameSignals.OnSubmitCurrentCollectibles += CurrentCollectibles;
    }
    
    private void OnDisable()
    {
        GameSignals.OnGameOver -= DestroyAll;
        GameSignals.OnTotalCollectible -= TotalCollectibles;
        GameSignals.OnSubmitCurrentCollectibles -= CurrentCollectibles;
    }
}
