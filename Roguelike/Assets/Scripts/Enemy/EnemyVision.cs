using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float detectionRange = 10f;
    public float viewAngle = 90f;
    public int segments = 20;

    public LayerMask obstacleLayer;
    public Transform player;

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (!player) return;

        DrawFOV();

        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > detectionRange)
            return;

        directionToPlayer.Normalize();

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > viewAngle / 2f)
            return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, distance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, directionToPlayer * distance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, directionToPlayer * distance, Color.green);
            Debug.Log("PLAYER DETECTED");
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