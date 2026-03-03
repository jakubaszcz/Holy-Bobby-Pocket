using System;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float detectionRange = 10f;
    
    public LayerMask obstacleLayer;

    public Transform player;

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, distance, obstacleLayer))
            {
                Debug.DrawRay(transform.position, direction * distance, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, direction * distance, Color.green);
            }
        }
    }
}