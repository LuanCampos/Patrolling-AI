using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Patrol : MonoBehaviour
{
    [Header("Defines Patrol Area:")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;
    
    private NavMeshAgent agent;
    private float stoppingDistance;
    
    void Start()
    {
        FindAgent();
        GetStoppingDistance();
        SetRandomDestination();
    }

    void Update()
    {
        if (HasArrive())
            SetRandomDestination();
    }
    
    void OnEnable()
    {
        if (agent)
            agent.stoppingDistance = 0f;
    }
    
    void OnDisable()
    {
        if (agent)
            agent.stoppingDistance = stoppingDistance;
    }
    
    private void FindAgent()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void GetStoppingDistance()
    {
        stoppingDistance = agent.stoppingDistance;
    }
    
    private bool HasArrive()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    
    private void SetRandomDestination()
    {
        SetDestination(GetRandomPointInPatrolArea());
    }
    
    private void SetDestination(Vector3 goal)
    {
        agent.SetDestination(goal);
    }
    
    private Vector3 GetRandomPointInPatrolArea()
    {
        float x = Random.Range(-.5f,.5f) * size.x;
        float y = Random.Range(-.5f,.5f) * size.y;
        float z = Random.Range(-.5f,.5f) * size.z;
        
        return center + new Vector3(x, y, z);
    }
    
    void OnDrawGizmosSelected()
    {
        DrawPatrolArea();
        
        if (agent)
            DrawDestinationPoint();
    }
    
    private void DrawPatrolArea()
    {
        Gizmos.color = new Color(1, 1, 0, .75F);
        Gizmos.DrawWireCube(center, size);
    }
    
    private void DrawDestinationPoint()
    {
        Gizmos.color = new Color(1, 0, 0, .5F);
        Gizmos.DrawSphere(agent.destination + new Vector3(0,.5f,0), .25f);
    }
}