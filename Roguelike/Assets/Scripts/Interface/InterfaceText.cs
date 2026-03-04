using UnityEngine;
using TMPro;

public class InterfaceText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TMP_Text textObjectives;

    private void Start()
    {
        textObjectives.text = "Collect all the collectibles 0/0";
    }

    private void TotalCollectibles(int value)
    {
        textObjectives.text = "Collect all the collectibles 0/" + value;
    }

    private void OnEnable()
    {
        GameSignals.OnTotalCollectible += TotalCollectibles;
    }
    
    private void OnDisable()
    {
        GameSignals.OnTotalCollectible += TotalCollectibles;
    }
}
