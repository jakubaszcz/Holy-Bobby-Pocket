using UnityEngine;
using TMPro;

public class InterfaceText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TMP_Text textObjectives;
    [SerializeField] private TMP_Text textTimeRemaining;

    [SerializeField]
    private int maxCollectibles;

    private void Start()
    {
        textObjectives.text = "Collect all the collectibles 0/5";
        textTimeRemaining.text = "";
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

    private void OnEndGameTimer(int value)
    {
        textTimeRemaining.text = "You have " + value + " seconds to find the exit !";
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
        GameSignals.OnEndGameTimer += OnEndGameTimer;
    }
    
    private void OnDisable()    
    {
        GameSignals.OnGameOver -= DestroyAll;
        GameSignals.OnTotalCollectible -= TotalCollectibles;
        GameSignals.OnSubmitCurrentCollectibles -= CurrentCollectibles;
    }
}
