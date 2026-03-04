using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool _collected = false;

    public void Collect()
    {
        if (_collected) return;
        _collected = true;

        GameSignals.TriggerOnCurrentCollectibles(1);
        Destroy(gameObject);
    }
}
