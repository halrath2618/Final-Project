using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 15f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Combat")]
    public float attackRange = 3f;
    public float attackRadius = 1.5f;
    public LayerMask enemyLayer;

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator _animator;
    private Vector3 _velocity;
    private float _currentSpeed;
    private bool _isGrounded;
    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _currentSpeed = walkSpeed;
    }

    void Update()
    {
        // Ground check
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;

        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Sprint
        _currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Camera-relative movement
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            _controller.Move(moveDir.normalized * _currentSpeed * Time.deltaTime);

            // Smooth rotation
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            _animator.SetTrigger("Jump");
        }

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // Basic Attack
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }

        // Update animator
        _animator.SetFloat("Speed", direction.magnitude * _currentSpeed);
        _animator.SetBool("IsGrounded", _isGrounded);
    }

    void PerformAttack()
    {
        _animator.SetTrigger("Attack");

        // Detect enemies in front
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange / 2, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit: " + enemy.name);
            // Add damage logic here
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + transform.forward * attackRange / 2,
            attackRadius
        );
    }
}