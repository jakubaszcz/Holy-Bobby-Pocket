using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact")] 
    [SerializeField] private float range = 5f;
    [SerializeField] private float raycastRadius = 0.2f;
    [SerializeField] private LayerMask mask;
    private InputSystemActions _inputSystemActions;
    [SerializeField] private Camera camera;
    
    private float _interactionTimer = 0f;
    private const float InteractionTime = 3f;
    private bool _isInteracting = false;
    private Collectible _currentCollectible;

    void Awake()
    {
        _inputSystemActions = new InputSystemActions();
    }

    void Update()
    {
        if (_isInteracting)
        {
            // Vérifier si on regarde toujours le collectible
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 origin = ray.origin - ray.direction * 0.5f;
            float adjustedRange = range + 0.5f;

            if (Physics.SphereCast(origin, raycastRadius, ray.direction, out var hit, adjustedRange, mask))
            {
                Collectible hitCollectible = hit.collider.GetComponentInParent<Collectible>();
                if (hitCollectible != _currentCollectible)
                {
                    StopInteraction();
                    return;
                }
            }
            else
            {
                StopInteraction();
                return;
            }

            _interactionTimer += Time.deltaTime;
            GameSignals.TriggerOnCollectingCollectible(_interactionTimer / InteractionTime);

            if (_interactionTimer >= InteractionTime)
            {
                if (_currentCollectible)
                {
                    _currentCollectible.Collect();
                }
                StopInteraction();
            }
        }
    }

    private void StartInteraction(InputAction.CallbackContext ctx)
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 origin = ray.origin - ray.direction * 0.5f;
        float adjustedRange = range + 0.5f;

        if (Physics.SphereCast(origin, raycastRadius, ray.direction, out var hit, adjustedRange, mask))
        {
            _currentCollectible = hit.collider.GetComponentInParent<Collectible>();

            if (_currentCollectible)
            {
                _isInteracting = true;
                _interactionTimer = 0f;
            }
        }
    }

    private void StopInteraction(InputAction.CallbackContext ctx)
    {
        StopInteraction();
    }

    private void StopInteraction()
    {
        _isInteracting = false;
        _interactionTimer = 0f;
        _currentCollectible = null;
        GameSignals.TriggerOnCollectingCollectible(0f);
    }
    
    private void OnEnable()
    {
        _inputSystemActions.Enable();
        
        // Interact
        _inputSystemActions.Player.Interact.started += StartInteraction;
        _inputSystemActions.Player.Interact.canceled += StopInteraction;
    }
    
    private void OnDisable()
    {
        _inputSystemActions.Disable();
        
        // Interact
        _inputSystemActions.Player.Interact.started -= StartInteraction;
        _inputSystemActions.Player.Interact.canceled -= StopInteraction;
    }

}
