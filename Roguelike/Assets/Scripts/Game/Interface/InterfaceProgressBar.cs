using UnityEngine;
using UnityEngine.UI;

public class InterfaceProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;

    private void Start()
    {
        progressBarImage.fillAmount = 0f;
    }

    private void DestroyAll(GameSignals.GameOver gameOverSignal)
    {
        Destroy(progressBarImage);
    }
    
    private void OnEnable()
    {
        GameSignals.OnGameOver += DestroyAll;
        GameSignals.OnSeenValueChanged += UpdateProgressBar;
    }

    private void OnDisable()
    {
        GameSignals.OnGameOver -= DestroyAll;
        GameSignals.OnSeenValueChanged -= UpdateProgressBar;
    }

    private void UpdateProgressBar(float value)
    {
        if (progressBarImage != null)
        {
            progressBarImage.fillAmount = value;
        }
    }
}
