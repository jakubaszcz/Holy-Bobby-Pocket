using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private int totalCollectibles;
    [SerializeField] private int currentCollectibles;
    [SerializeField] private int spotted;

    [SerializeField] private bool hasTimerStart;
    [SerializeField] private float timer;
    [SerializeField] private bool hasEndGameTimerStart;
    [SerializeField] private float endGameTimer = 20f;
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

        if (hasEndGameTimerStart)
        {
            endGameTimer -= Time.deltaTime;

            if (endGameTimer <= 0f)
            {
                            
                endGameTimer = 0f;
                GameSignals.TriggerGameOver(GameSignals.GameOver.Lose);
            }
            
            GameSignals.TriggerEndGameTimer((int)endGameTimer);
        }
    }
    
    private void OnStartTimer(bool value)
    {
        hasTimerStart = value;
    }

    private void OnEndGame(bool value)
    {
        hasEndGameTimerStart = value;
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

    public void CalculateRank()
    {
        int totalSeconds = (int)timer;

        int score = 1000;

        score -= totalSeconds * 2;
        score -= spotted * 50;

        GameSignals.Rank rank;

        if (score >= 900)
            rank = GameSignals.Rank.S;
        else if (score >= 700)
            rank = GameSignals.Rank.A;
        else if (score >= 500)
            rank = GameSignals.Rank.B;
        else if (score >= 300)
            rank = GameSignals.Rank.C;
        else
            rank = GameSignals.Rank.D;

        GameSignals.TriggerRank(rank);
    }
    
    public void OnGameOver(GameSignals.GameOver value)
    {
        if (value == GameSignals.GameOver.Win)
            CalculateRank();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    
    void HandleTotal(int value) {
        totalCollectibles = value;
    }
}
