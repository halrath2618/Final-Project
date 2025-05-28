using UnityEngine;

public class TERAStyleCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 3f;
    public float scrollSpeed = 2f;

    [Header("Camera Settings")]
    public Transform player;
    public float distanceFromPlayer = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float verticalClampMin = -30f;
    public float verticalClampMax = 60f;

    public float currentYaw = 0f;
    private float currentPitch = 15f;
    private float currentDistance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = distanceFromPlayer;
    }

    void Update()
    {
        HandleMouseLook();
        HandleZoom();
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    void HandleMouseLook()
    {
        if (Input.GetMouseButton(1))  // Right Mouse Button
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            currentYaw += mouseX;
            currentPitch -= mouseY;
            currentPitch = Mathf.Clamp(currentPitch, verticalClampMin, verticalClampMax);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        currentDistance = Mathf.Clamp(currentDistance - scroll, minDistance, maxDistance);
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -currentDistance);

        transform.position = player.position + offset;
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}