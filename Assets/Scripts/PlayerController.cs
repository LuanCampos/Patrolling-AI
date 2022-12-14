using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    [Header("[Movement Variables]")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 4.5f;
    [SerializeField] private float lookAtSpeed = .3f;
    
    private Transform mainCamera;
    private Vector3 input;
    private Rigidbody rb;
    private bool running;
    private Vector3 zeroY = new Vector3(1, 0, 1);
    
    void Start()
    {
        GetReferences();
    }
    
    void Update()
    {
        GetInput();
    }
    
    void FixedUpdate()
    {
        Move();
    }
    
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.gameObject.transform;
    }
    
    private void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");
        running = Input.GetKey(KeyCode.LeftShift) ? true : false;
    }
    
    private void Move()
    {
        Vector3 camForward = Vector3.Scale(mainCamera.forward, zeroY).normalized;
        Vector3 direction = (input.z * camForward + input.x * mainCamera.right).normalized;
        direction *= running ? runSpeed : walkSpeed;
        
        Vector3 velocityChange = (direction - rb.velocity);
        velocityChange.y = 0;
 
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        
        if (direction.magnitude > 0f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), lookAtSpeed);
    }
}
