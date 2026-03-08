using System;
using UnityEngine;

public class GameSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.Play();
    }

    private void OnEndGame(bool value)
    {
        _audioSource.Stop();
    }
    
    private void OnGameOver(GameSignals.GameOver gameOverSignal)
    {
        _audioSource.Stop();
        
        if (gameOverSignal == GameSignals.GameOver.Win)
        {
            winSound.Play();
        }
        else if (gameOverSignal == GameSignals.GameOver.Lose)
        {
            loseSound.Play();
        }
    }
    private void OnEnable()
    {
        GameSignals.OnEndGame += OnEndGame;
        GameSignals.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        GameSignals.OnEndGame -=  OnEndGame;
        GameSignals.OnGameOver -= OnGameOver;
    }
}
