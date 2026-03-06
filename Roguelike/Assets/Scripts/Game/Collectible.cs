using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool _collected = false;
    [Header("SFX")]
    [SerializeField] private AudioSource audio;

    public void Collect()
    {
        if (_collected) return;
        _collected = true;

        AudioSource.PlayClipAtPoint(audio.clip, transform.position);
        
        GameSignals.TriggerOnCurrentCollectibles(1);
        Destroy(gameObject);
    }
}
