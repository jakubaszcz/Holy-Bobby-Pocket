using UnityEngine;

public class AlarmSFX : MonoBehaviour
{

    [SerializeField] private AudioSource audio;
    
    private void OnEndGame(bool value)
    {
        audio.Play();
    }

    public void OnGameOver(GameSignals.GameOver gameOverSignal)
    {
        audio.Stop();
    }
    private void OnEnable()
    {
        GameSignals.OnEndGame += OnEndGame;
        GameSignals.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        GameSignals.OnEndGame -= OnEndGame;
        GameSignals.OnGameOver += OnGameOver;
    }
}
