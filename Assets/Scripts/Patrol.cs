using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Patrol : MonoBehaviour
{
    [Header("Defines Patrol Area:")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;
    
    private NavMeshAgent agent;
    private float originalStopDist;
    
    void Start()
    {
        FindAgent();
        GetOriginalStoppingDistance();
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
            SetStoppingDistance(0f);
    }
    
    void OnDisable()
    {
        if (agent)
            SetStoppingDistance(originalStopDist);
    }
    
    void OnDrawGizmosSelected()
    {
        DrawPatrolArea();
        
        if (agent)
            DrawDestinationPoint();
    }
    
    private void FindAgent()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void GetOriginalStoppingDistance()
    {
        originalStopDist = agent.stoppingDistance;
    }
    
    private void SetRandomDestination()
    {
        agent.SetDestination(GetRandomPointInPatrolArea());
    }
    
    private Vector3 GetRandomPointInPatrolArea()
    {
        float x = Random.Range(-.5f,.5f) * size.x;
        float y = Random.Range(-.5f,.5f) * size.y;
        float z = Random.Range(-.5f,.5f) * size.z;
        
        return center + new Vector3(x, y, z);
    }
    
    private bool HasArrive()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    
    private void SetStoppingDistance(float newStopDist)
    {
        agent.stoppingDistance = newStopDist;
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