using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private int totalCollectibles;
    [SerializeField] private int currentCollectibles;
    [SerializeField] private int spotted;

    [SerializeField] private bool hasTimerStart;
    [SerializeField] private float timer;

    private void Start()
    {
        hasTimerStart = false;
    }

    private void Update()
    {
        if (hasTimerStart)
        {
            timer += Time.deltaTime;
            GameSignals.TriggerOnTimer((int)timer);
        }
    }
    
    private void OnStartTimer(bool value)
    {
        hasTimerStart = value;
    }
    
    private void OnEnable()
    {
        GameSignals.OnStartTimer += OnStartTimer;
        GameSignals.OnSpot += OnSpot;
        GameSignals.OnCurrentCollectibles += HandleCollectibles;
        GameSignals.OnTotalCollectible += HandleTotal;
        GameSignals.OnGameOver += OnGameOver;
        
    }
    
    private void OnDisable()
    {
        GameSignals.OnSpot -= OnSpot;
        GameSignals.OnTotalCollectible -= HandleTotal;
        GameSignals.OnCurrentCollectibles -= HandleCollectibles;
        GameSignals.OnGameOver -= OnGameOver;
    }

    void OnSpot(int value)
    {
        spotted += value;
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
