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

    void Awake()
    {
        _inputSystemActions = new InputSystemActions();
    }
    void CheckInteract(InputAction.CallbackContext ctx)
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 origin = ray.origin - ray.direction * 0.5f;
        float adjustedRange = range + 0.5f;

        if (Physics.SphereCast(origin, raycastRadius, ray.direction, out var hit, adjustedRange, mask))
        {
            Debug.Log("Interact with: " + hit.collider.name);
            Collectible collectible = hit.collider.GetComponentInParent<Collectible>();

            if (collectible)
            {
                collectible.Collect();
                return;
            }
            else
            {
                Debug.Log("Nothing to collect on " + hit.collider.name);
            }
        }
    }
    
    private void OnEnable()
    {
        _inputSystemActions.Enable();
        
        // Interact
        _inputSystemActions.Player.Interact.performed += CheckInteract;
    }
    
    private void OnDisable()
    {
        _inputSystemActions.Disable();
        
        // Interact
        _inputSystemActions.Player.Interact.performed -= CheckInteract;
    }

}
