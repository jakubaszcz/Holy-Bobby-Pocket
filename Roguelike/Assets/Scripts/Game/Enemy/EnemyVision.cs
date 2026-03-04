using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float detectionRange = 10f;
    public float viewAngle = 90f;
    public int segments = 20;

    public LayerMask obstacleLayer;
    public Transform player;

    private PlayerStatistics playerStatistics;
    private bool isDetectingPlayer = false;

    public bool getIsDetectingPlayer()
    {
        return isDetectingPlayer;
    }
    
    public void SetPlayer(Transform player)
    {
        this.player = player;
        playerStatistics = player.GetComponent<PlayerStatistics>();
    }

    private void OnDisable()
    {
        if (isDetectingPlayer)
        {
            isDetectingPlayer = false;
            GameSignals.SetEnemySeeing(false);
        }
    }

    private void Update()
    {
        if (!player) return;

        DrawFOV();

        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;
        bool playerDetected = false;

        if (distance <= detectionRange)
        {
            Vector3 direction = directionToPlayer.normalized;
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle <= viewAngle / 2f)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, direction, out hit, distance, obstacleLayer))
                {
                    playerDetected = true;
                }
            }
        }

        if (playerDetected)
        {
            Debug.DrawRay(transform.position, directionToPlayer, Color.green);
            if (!isDetectingPlayer)
            {
                isDetectingPlayer = true;
                GameSignals.SetEnemySeeing(true);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, directionToPlayer, Color.red);
            if (isDetectingPlayer)
            {
                isDetectingPlayer = false;
                GameSignals.SetEnemySeeing(false);
            }
        }
    }

    private void DrawFOV()
    {
        float angleStep = viewAngle / segments;
        float startAngle = -viewAngle / 2f;

        Vector3 previousPoint = transform.position;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + angleStep * i;

            Vector3 direction =
                Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            Vector3 point =
                transform.position + direction * detectionRange;

            Debug.DrawLine(transform.position, point, Color.yellow);

            if (i > 0)
                Debug.DrawLine(previousPoint, point, Color.yellow);

            previousPoint = point;
        }
    }
}