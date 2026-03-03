using System;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [Header("Player statistics")]
    [SerializeField] private float maxTimeSeen = 3f;
    [SerializeField] private float currentSeen = 0f;
    [SerializeField] private bool isSeen;

    private void Start()
    {
        isSeen = false;
    }

    public void SetSeen(bool value)
    {
        isSeen = value;
    }
    
    private void Update()
    {
        if (isSeen)
        {
            currentSeen += Time.deltaTime;
            
            if (currentSeen >= maxTimeSeen)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (currentSeen > 0f)
            {
                currentSeen -= Time.deltaTime;
                if (currentSeen < 0f)
                {
                    currentSeen = 0f;
                }
            }
        }
    }
}
