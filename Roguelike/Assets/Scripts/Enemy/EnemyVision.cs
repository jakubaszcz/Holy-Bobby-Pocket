using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("View Settings")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    
    [SerializeField] private Transform player;

    public void SetPlayer(Transform transform)
    {
        player = transform;
    }
    
    private void Update()
    {
        if (SeenPlayer()) Debug.Log("Seen Player");
    }

    private bool SeenPlayer()
    {
        
        Vector3 directionToTarget = (player.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, player.position);

        if (distanceToTarget > viewDistance)
        {
            return false;
        }
        
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        if (angle > viewAngle / 2f)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget, obstacleMask))
        {
            Debug.DrawRay(transform.position, directionToTarget * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.green);
            return true;
        }

        return false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
    }
}
