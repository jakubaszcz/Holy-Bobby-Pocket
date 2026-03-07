using UnityEngine;

public class PlayerInterface : MonoBehaviour
{

    [SerializeField] private GameObject _gameObject;

    private void Awake()
    {
        _gameObject.SetActive(false);
    }
    public void IsInRange(bool value)
    {
        _gameObject.SetActive(value);
    }
    public void OnEnable()
    {
        GameSignals.IsInRange += IsInRange;
    }
    
    public void OnDisable()
    {
        GameSignals.IsInRange -= IsInRange;
    }
}
