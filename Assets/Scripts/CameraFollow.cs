using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Camera Positioning:")]
    [SerializeField] Vector3 offset;
	
	private Camera cam;
    private Transform player;
    
    void Start()
    {
        SetVariables();
        SetInitialPositioning();
    }
	
	void Update()
	{
		SetZoom();
	}

    void LateUpdate()
    {
        FollowThePlayer();
    }
    
    private void SetVariables()
    {
		cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void SetInitialPositioning()
    {
        transform.position = offset;
    }
	
	private void SetZoom()
	{
		if (Input.mouseScrollDelta.y < 0 && Vector3.Distance(player.position, transform.position) < 50f)
			offset += offset.normalized;
		
		if (Input.mouseScrollDelta.y > 0 && Vector3.Distance(player.position, transform.position) > 5f)
			offset -= offset.normalized;
	}
    
    private void FollowThePlayer()
    {
        transform.position = PlayerPosition() + offset;
		transform.LookAt(player);
    }
    
    private Vector3 PlayerPosition()
    {
        return player.position;
    }
}