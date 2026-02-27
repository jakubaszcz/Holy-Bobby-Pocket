using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystemActions _inputSystemActions;
    
    [Header("Player characteristics")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 20f;
    
    [Header("Player directions")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Vector2 movement;
    
    [SerializeField] private bool isSprint;

    private void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    
    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprint = context.ReadValueAsButton();
    }
    
    
    private void Awake()
    {
        isSprint = false;
        
        _inputSystemActions = new InputSystemActions();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        float speed = isSprint ? sprintSpeed : walkSpeed;
    
        Vector3 direction =
            transform.right * movement.x +
            transform.forward * movement.y;

        Vector3 velocity = direction * speed;

        rigidbody.linearVelocity = new Vector3(
            velocity.x,
            rigidbody.linearVelocity.y,
            velocity.z
        );
    }
    
    private void OnEnable()
    {
        _inputSystemActions.Enable();
        
        // Movement
        _inputSystemActions.Player.Move.performed += OnMovement;
        _inputSystemActions.Player.Move.canceled += OnMovement;
        
        // Run
        _inputSystemActions.Player.Sprint.performed += OnSprint;
        _inputSystemActions.Player.Sprint.canceled += OnSprint;

    }
    
    private void OnDisable()
    {
        _inputSystemActions.Disable();
        
        // Movement
        _inputSystemActions.Player.Move.performed += OnMovement;
        _inputSystemActions.Player.Move.canceled += OnMovement;
        
        // Run
        _inputSystemActions.Player.Sprint.performed -= OnSprint;
        _inputSystemActions.Player.Sprint.canceled -= OnSprint;
    }
    
}
