using System;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private float _targetVolume = 1.0f;
    private Coroutine _fadeCoroutine;

    private void OnSeenValueChanged(bool value)
    {
        if (value)
        {
            if (_audioSource.isPlaying) return;
            StartCrescendo();
        }
        else
        {
            if (!_audioSource.isPlaying) return;
            StopCrescendo();
        }
    }

    private void StartCrescendo()
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeAudio(0.03f, _targetVolume, _fadeDuration));
    }

    private void StopCrescendo()
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _audioSource.Stop();
    }

    private System.Collections.IEnumerator FadeAudio(float startVolume, float endVolume, float duration)
    {
        _audioSource.volume = startVolume;
        _audioSource.Play();
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        _audioSource.volume = endVolume;
    }

    private void OnGameOver(GameSignals.GameOver gameOverSignal)
    {
        StopCrescendo();
    }
    
    private void OnEnable()
    {
        GameSignals.IsSeenChanged += OnSeenValueChanged;
        GameSignals.OnGameOver += OnGameOver; 
    }
    
    private void OnDisable()
    {
        GameSignals.IsSeenChanged -= OnSeenValueChanged;
        GameSignals.OnGameOver -= OnGameOver;
    }
}
