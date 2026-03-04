using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float roamRadius = 20f;

    [Header("Idle Rotation")]
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float rotateMaxAngle = 45f;
    [SerializeField] private float idleTime = 1.5f;

    private NavMeshAgent _agent;
    private EnemyVision _enemyVision;
    private Transform _player;

    private enum State { Roaming, IdleRotating, Chasing }
    private State _state = State.IdleRotating;

    private float _idleTimer;
    private float _rotationAngle;
    private float _rotationDirection = 1f;
    private Quaternion _baseRotation;

    private void Start()
    {
        
        roamRadius = Random.Range(15f, 40f);
        
        _agent = GetComponent<NavMeshAgent>();
        _enemyVision = GetComponent<EnemyVision>();
        StartIdle();
    }

    public void SetPlayer(Transform player) => _player = player;

    private void Update()
    {
        if (!_player) return;

        if (_enemyVision != null && _enemyVision.getIsDetectingPlayer())
        {
            _state = State.Chasing;
        }
        else if (_state == State.Chasing)
        {
            StartIdle();
        }

        switch (_state)
        {
            case State.Chasing:      Chase();        break;
            case State.Roaming:      CheckArrival(); break;
            case State.IdleRotating: IdleRotate();   break;
        }
    }

    private void Chase()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
    }

    private void CheckArrival()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            StartIdle();
    }

    private void StartIdle()
    {
        _state = State.IdleRotating;
        _agent.isStopped = true;
        _idleTimer = idleTime;
        _rotationAngle = 0f;
        _rotationDirection = 1f;
        _baseRotation = transform.rotation;
    }

    private void IdleRotate()
    {
        _rotationAngle += rotateSpeed * _rotationDirection * Time.deltaTime;

        if (_rotationAngle >= rotateMaxAngle)
        {
            _rotationAngle = rotateMaxAngle;
            _rotationDirection = -1f;
        }
        else if (_rotationAngle <= -rotateMaxAngle)
        {
            _rotationAngle = -rotateMaxAngle;
            _rotationDirection = 1f;
        }

        transform.rotation = _baseRotation * Quaternion.Euler(0, _rotationAngle, 0);

        if (_rotationDirection == -1f)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0f)
                PickNewDestination();
        }
    }

    private void PickNewDestination()
    {
        Vector3 randomDir = Random.insideUnitSphere * roamRadius + transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, roamRadius, NavMesh.AllAreas))
        {
            _state = State.Roaming;
            _agent.isStopped = false;
            _agent.SetDestination(hit.position);
        }
        else
        {
            StartIdle();
        }
    }
}
