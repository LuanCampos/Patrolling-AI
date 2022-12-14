using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Follow : MonoBehaviour
{
    [Header("Defines Target:")]
    [SerializeField] private Transform target;
    
    [Header("Defines Search Parameters:")]
    [SerializeField] [Range(0, 100)] private float visionDepth;
    [SerializeField] [Range(0, 360)] private float visionAngle;
    
    [Header("Editor Visualization:")]
    [SerializeField] [Range(2, 200)] private int raycastAmount;
    
    private NavMeshAgent agent;
    private Patrol patrol;
    
    void Start()
    {
        CheckTarget();
        GetReferences();
    }

    void Update()
    {
        LookForTarget();
    }
    
    void OnDrawGizmosSelected()
    {
        DrawVision();
    }
    
    private void CheckTarget()
    {
        if (!target)
            DestroyThis();
    }
    
    private void DestroyThis()
    {
        Debug.LogWarning("Follow behaviour demands a target! (" + this.gameObject.name + ")");
        this.enabled = false;
    }
    
    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        patrol = GetComponent<Patrol>();
    }
    
    private void LookForTarget()
    {
        if (TargetIsOnVisionArea() && CanSeeTarget())
            FollowTarget();
        else
            ContinueToDestination();
    }
    
    private bool TargetIsOnVisionArea()
    {
        return TargetDistance() <= visionDepth && TargetAngle() <= visionAngle / 2f;
    }

    private float TargetDistance()
    {
        return Vector3.Distance(transform.position, target.position);
    }
    
    private float TargetAngle()
    {
        return Vector3.Angle(target.position - transform.position, transform.forward);
    }
    
    private void ContinueToDestination()
    {
        if (!HasArrive())
            return;
        
        if (CanSeeTarget())
            FollowTarget();
        else
            SetPatrol(true);
    }
    
    private bool CanSeeTarget()
    {
        Ray ray = new Ray(transform.position, target.position - transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, visionDepth);
        
        if (hit.transform == target)
            return true;
        
        return false;
    }
    
    private void FollowTarget()
    {
        agent.SetDestination(target.position);
        SetPatrol(false);
    }
    
    private bool HasArrive()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    
    private void SetPatrol(bool isEnable)
    {
        if (patrol)
            patrol.enabled = isEnable;
    }
    
    private void DrawVision()
    {
        Gizmos.color = new Color(1, 0, 0, .3F);
        
        for (int i=0; i < raycastAmount; i++)
        {           
            Quaternion rayRotation = Quaternion.AngleAxis(GetRayAngle(i), Vector3.up);
            Vector3 rayVector = rayRotation * transform.forward;    
            Gizmos.DrawRay(transform.position, GetRayVectorScaled(rayVector));
        }
        
        if (TargetIsOnVisionArea() && CanSeeTarget())
            DrawRayToTarget();
    }
    
    private float GetRayAngle(int i)
    {
        float initial = -visionAngle / 2f;
        float variation = i * (visionAngle / (raycastAmount -1));
        return initial + variation;
    }
    
    private Vector3 GetRayVectorScaled(Vector3 rayVector)
    {
        Ray ray = new Ray(transform.position, rayVector);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, visionDepth);
        float scale = hit.distance > 0 ? hit.distance : visionDepth;
        return rayVector * scale;
    }
    
    private void DrawRayToTarget()
    {
        Gizmos.color = new Color(1, 1, 0, .75F);
        Gizmos.DrawRay(transform.position, target.position - transform.position);
    }
}