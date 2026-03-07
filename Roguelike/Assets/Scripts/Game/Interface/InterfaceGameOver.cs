using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InterfaceGameOver : MonoBehaviour
{
    [Header("Game Over Container")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text textRank;
    

    private void Start()
    {
        panel.SetActive(false);
    }

    private void OnGameOver(GameSignals.GameOver gameOverSignal)
    {
        panel.SetActive(true);
        if (gameOverSignal == GameSignals.GameOver.Win)
        {
            text.text = "You Win!";
        }
        else
        {
            text.text = "You Lose!";
        }
        
    }

    public void OnMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void OnRestart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void OnExit()
    {
        Application.Quit();
    }

    public void OnRank(GameSignals.Rank rank)
    {
        switch (rank)
        {
            case GameSignals.Rank.D:
                textRank.text = "D";
                break;
            case GameSignals.Rank.C:
                textRank.text = "C";
                break;
            case GameSignals.Rank.B:
                textRank.text = "B";
                break;
            case GameSignals.Rank.A:
                textRank.text = "A";
                break;
            case GameSignals.Rank.S:
                textRank.text = "S";
                break;
            default:
                textRank.text = "N/A";
                break;
        }
    }
    
    private void OnEnable()
    {
        GameSignals.OnGameOver += OnGameOver;
        GameSignals.OnRank += OnRank;
    }
    
    private void OnDisable()
    {
        GameSignals.OnGameOver -= OnGameOver;
        GameSignals.OnRank -= OnRank;
        
    }
}
