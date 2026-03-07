using UnityEngine;

public class TrapPlayerBehaviour : MonoBehaviour
{
    
    [SerializeField] private bool trapActive = true;
    
    [SerializeField] private float timeUntilUnTrap = 3f;
    [SerializeField] private AudioSource audio;

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
            audio.Play();
            GameSignals.TriggerTrapped(true);

            trapActive = false;
            timer = timeUntilUnTrap;
        }
    }
}
