using UnityEngine;
using UnityEngine.UI;

public class InterfaceProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBarSeenImage;
    [SerializeField] private Image progressBarCollectImage;

    private void Start()
    {
        progressBarSeenImage.fillAmount = 0f;
        progressBarCollectImage.fillAmount = 0f;
    }

    private void DestroyAll(GameSignals.GameOver gameOverSignal)
    {
        Destroy(progressBarSeenImage);
        Destroy(progressBarCollectImage);
    }
    
    private void OnEnable()
    {
        GameSignals.OnGameOver += DestroyAll;
        GameSignals.OnSeenValueChanged += UpdateSeenProgressBar;
        GameSignals.OnCollectingCollectible += UpdateCollectProgressBar;
    }

    private void OnDisable()
    {
        GameSignals.OnGameOver -= DestroyAll;
        GameSignals.OnSeenValueChanged -= UpdateSeenProgressBar;
        GameSignals.OnCollectingCollectible -= UpdateCollectProgressBar;
    }

    private void UpdateCollectProgressBar(float value)
    {
        if (progressBarCollectImage != null)
        {
            progressBarCollectImage.fillAmount = value;
        }
    }

    private void UpdateSeenProgressBar(float value)
    {
        if (progressBarSeenImage != null)
        {
            progressBarSeenImage.fillAmount = value;
        }
    }
}
