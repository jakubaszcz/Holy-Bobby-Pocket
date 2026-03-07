using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private int totalCollectibles;
    [SerializeField] private int currentCollectibles;
    [SerializeField] private int spotted;

    [SerializeField] private bool hasTimerStart;
    [SerializeField] private float timer;
    [SerializeField] private Light spotLight;

    private void Awake()
    {
        Time.timeScale = 1f;
    }
    
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

    private void OnEndGame(bool value)
    {
        spotLight.color = Color.red;
        spotLight.intensity = 10f;
    }
    
    private void OnEnable()
    {
        GameSignals.OnStartTimer += OnStartTimer;
        GameSignals.OnSpot += OnSpot;
        GameSignals.OnCurrentCollectibles += HandleCollectibles;
        GameSignals.OnTotalCollectible += HandleTotal;
        GameSignals.OnEndGame += OnEndGame;
        GameSignals.OnGameOver += OnGameOver;
        
    }
    
    private void OnDisable()
    {
        GameSignals.OnSpot -= OnSpot;
        GameSignals.OnTotalCollectible -= HandleTotal;
        GameSignals.OnCurrentCollectibles -= HandleCollectibles;
        GameSignals.OnEndGame -= OnEndGame;
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
            GameSignals.TriggerOnEndGame(true);
        }
        
        GameSignals.TriggerOnSubmitCollectibles(currentCollectibles);
    }

    public void OnGameOver(GameSignals.GameOver value)
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    
    void HandleTotal(int value) {
        totalCollectibles = value;
    }
}
