using UnityEngine;
using TMPro;

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
    
    private void OnEnable()
    {
        GameSignals.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        GameSignals.OnGameOver -= OnGameOver;
    }
}
