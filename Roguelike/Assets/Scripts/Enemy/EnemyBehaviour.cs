using UnityEngine;
using UnityEngine.U2D;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemies")] [SerializeField] private float rotateTime;
    [SerializeField] private float idleTime;

    private float currentIdleTime;
    [SerializeField] private float rotateSpeed = 45f;
    [SerializeField] private float rotateMaxAngle = 45f;

    [SerializeField] private bool rotateLeft = true;
    
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isRotating;
    [SerializeField] private bool isReturning;

    private Quaternion targetRotation;
    private Quaternion initialRotation;

    private void Start()
    {
        // Boolean
        isIdle = true;
        isRotating = false;
        isReturning = false;
        
        initialRotation = transform.rotation;
        
        // Current
        currentIdleTime = 0f;
        
        rotateTime = Random.Range(1f, 2f);
        idleTime = Random.Range(1f, 2.5f);
    }

    private void Update()
    {
        UpdateIdle();
        UpdateRotate();
    }

    private void UpdateRotate()
    {
        if (isRotating)
        {
            float angle = rotateLeft ? rotateMaxAngle : -rotateMaxAngle;
            targetRotation = initialRotation * Quaternion.Euler(0, angle, 0);
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isRotating = false;
                isIdle = true;
                currentIdleTime = 0f;
                isReturning = true;
            }
        }
        else if (isReturning)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, rotateSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, initialRotation) < 0.1f)
            {
                isReturning = false;
                isIdle = true;
                currentIdleTime = 0f;
                rotateLeft = !rotateLeft;
            }
        }
    }
    
    private void UpdateIdle()
    {
        if (isIdle && !isRotating && !isReturning)
        {
            currentIdleTime += Time.deltaTime;

            if (currentIdleTime >= idleTime)
            {
                isIdle = false;
                currentIdleTime = 0f;
                isRotating = true;
            }
        }
    }
    
    
}
