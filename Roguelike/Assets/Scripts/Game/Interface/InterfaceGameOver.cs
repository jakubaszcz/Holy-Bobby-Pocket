using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InterfaceGameOver : MonoBehaviour
{
    [Header("Game Over Container")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text text;
    

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
    
    private void OnEnable()
    {
        GameSignals.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        GameSignals.OnGameOver -= OnGameOver;
    }
}
