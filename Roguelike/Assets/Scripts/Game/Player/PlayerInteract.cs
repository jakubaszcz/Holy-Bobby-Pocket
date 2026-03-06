using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact")] 
    [SerializeField] private float range = 5f;
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

        if (Physics.Raycast(ray, out var hit, range, mask))
        {
            Debug.Log("Interact");
            Collectible collectible = hit.collider.GetComponent<Collectible>();

            if (collectible)
            {
                collectible.Collect();

                return;
            }
            else
            {
                Debug.Log("Nothinh to collect");
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
