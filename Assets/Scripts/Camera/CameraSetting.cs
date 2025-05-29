using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, -5f);

    [Header("Settings")]
    public float rotationSpeed = 2f;
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 10f;
    public float collisionOffset = 0.3f;

    private float _currentZoom;
    private float _verticalAngle;
    private float _horizontalAngle;

    void Start()
    {
        _currentZoom = -offset.z;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Camera Rotation
        _horizontalAngle += Input.GetAxis("Mouse X") * rotationSpeed;
        _verticalAngle -= Input.GetAxis("Mouse Y") * rotationSpeed;
        _verticalAngle = Mathf.Clamp(_verticalAngle, -30f, 70f);

        // Camera Zoom
        _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);

        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0);
        Vector3 desiredPosition = target.position + rotation * new Vector3(0, 0, -_currentZoom);

        // Obstacle avoidance
        RaycastHit hit;
        if (Physics.Linecast(target.position, desiredPosition, out hit))
        {
            desiredPosition = hit.point + (target.position - desiredPosition).normalized * collisionOffset;
        }

        // Apply position/rotation
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}