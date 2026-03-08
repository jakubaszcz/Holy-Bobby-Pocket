using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystemActions _inputSystemActions;
    [SerializeField] private Transform camera;
    
    
    [Header("Stamina settings")]
    [SerializeField] private float staminaRegenRate = 0.5f;
    [SerializeField] private float staminaRecoveryDelay = 1f;
    private float _recoveryTimer = 0f;

    [Header("Player characteristics")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float sensitivity = 0.4f;
    [SerializeField] private bool isTrapped = false;
    
    [Header("Player directions")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Vector2 look;
    [SerializeField] private Vector2 movement;
    [SerializeField] private float xRotation;
    
    [SerializeField] private bool hasMovedForTheFirstTime = false;
    
    [SerializeField] private bool isSprint;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isTrapped = false;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        if (!hasMovedForTheFirstTime)
        {
            GameSignals.TriggerOnStartTimer(true);
            hasMovedForTheFirstTime = true;
        }
        movement = context.ReadValue<Vector2>();
    }
    
    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprint = context.ReadValueAsButton();
    }
    
    private void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
        
        float x = look.x * sensitivity;
        float y = look.y * sensitivity;
        
        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        camera.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);
        
        transform.Rotate(Vector3.up * x);
    }
    
    private void Awake()
    {
        isSprint = false;
        
        _inputSystemActions = new InputSystemActions();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isTrapped)
        {
            rigidbody.linearVelocity = Vector3.zero;
            return;
        }

        MovementUpdate();
    }

    private void MovementUpdate()
    {
        float speed = isSprint ? sprintSpeed : walkSpeed;
    
        Vector3 forward = camera.forward;
        Vector3 right = camera.right;
        
        Vector3 direction =
            right * movement.x +
            forward * movement.y;

        Vector3 velocity = direction * speed;

        rigidbody.linearVelocity = new Vector3(
            velocity.x,
            rigidbody.linearVelocity.y,
            velocity.z
        );
    }

    private void Ontrap(bool value)
    {
        isTrapped = value;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        
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
        
        // Look
        _inputSystemActions.Player.Look.performed += OnLook;
        
        // Trap
        GameSignals.OnTrapped += Ontrap;

    }
    
    private void OnDisable()
    {
        _inputSystemActions.Disable();
        
        // Movement
        _inputSystemActions.Player.Move.performed -= OnMovement;
        _inputSystemActions.Player.Move.canceled -= OnMovement;
        
        // Run
        _inputSystemActions.Player.Sprint.performed -= OnSprint;
        _inputSystemActions.Player.Sprint.canceled -= OnSprint;
        
        // Look
        _inputSystemActions.Player.Look.performed -= OnLook;
        
        // trap
        GameSignals.OnTrapped -= Ontrap;
    }
    
}
