using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Movement:")]    
    [Tooltip("Less that 1 to smoother movements, more than 1 to faster movements")]
    [SerializeField] private float smoothness = 1f;
    
	private Camera cam;
    private Transform player;
	private Vector3 offset;
    
    void Start()
    {
        SetVariables();
        SetInitialPositioning();
    }
	
	void Update()
	{
		GetZoomInput();
	}

    void LateUpdate()
    {
        FollowThePlayer();
    }
    
    private void SetVariables()
    {
		cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
    }
    
    private void SetInitialPositioning()
    {
        transform.position = offset;
    }
	
	private void GetZoomInput()
	{
		if (Input.mouseScrollDelta.y < 0 && Vector3.Distance(player.position, transform.position) < 50f)
			offset -= (player.position - transform.position).normalized;
		
		if (Input.mouseScrollDelta.y > 0 && Vector3.Distance(player.position, transform.position) > 5f)
			offset += (player.position - transform.position).normalized;
	}
    
    private void FollowThePlayer()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerPosition() + offset, smoothness);
		transform.LookAt(player);
    }
    
    private Vector3 PlayerPosition()
    {
        return new Vector3(player.position.x, player.position.y, player.position.z);
    }
}