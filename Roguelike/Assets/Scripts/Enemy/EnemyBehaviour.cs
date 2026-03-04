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
    [SerializeField] private float followSpeed = 10f;

    private Quaternion targetRotation;
    private Quaternion initialRotation;

    private EnemyVision _enemyVision;

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

        _enemyVision = GetComponent<EnemyVision>();
    }

    private void Update()
    {
        if (_enemyVision.getIsDetectingPlayer())
        {
            FollowPlayer();
            return;
        }
        UpdateIdle();
        UpdateRotate();
    }

    private void FollowPlayer()
    {
        if (_enemyVision.player == null) return;

        Vector3 directionToPlayer = _enemyVision.player.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, followSpeed * Time.deltaTime);
        }
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
