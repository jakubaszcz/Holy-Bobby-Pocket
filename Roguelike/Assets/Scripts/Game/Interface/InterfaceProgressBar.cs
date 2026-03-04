using UnityEngine;
using UnityEngine.UI;

public class InterfaceProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;

    private void Start()
    {
        progressBarImage.fillAmount = 0f;
    }
    
    private void OnEnable()
    {
        GameSignals.OnSeenValueChanged += UpdateProgressBar;
    }

    private void OnDisable()
    {
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
