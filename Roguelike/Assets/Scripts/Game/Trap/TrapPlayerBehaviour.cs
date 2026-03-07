using UnityEngine;

public class TrapPlayerBehaviour : MonoBehaviour
{
    
    [SerializeField] private bool trapActive = true;
    
    [SerializeField] private float timeUntilUnTrap = 3f;

    private float timer;

    private void Update()
    {
        if (!trapActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                trapActive = true;
                GameSignals.TriggerTrapped(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (trapActive)
        {
            GameSignals.TriggerTrapped(true);

            trapActive = false;
            timer = timeUntilUnTrap;
        }
    }
}
