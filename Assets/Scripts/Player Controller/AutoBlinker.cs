using UnityEngine;

public class AutoBlinker : MonoBehaviour
{
    [Header("Blend Shape Settings")]
    public SkinnedMeshRenderer faceMesh;
    public string blinkBlendShapeName = "Eye_Blink";
    [Tooltip("Full blink weight (usually 100)")]
    public float maxBlinkWeight = 100f;

    [Header("Blink Timing")]
    public float minBlinkInterval = 1f;
    public float maxBlinkInterval = 5f;
    [Tooltip("Duration of a single blink in seconds")]
    public float blinkDuration = 0.15f;

    private int blinkBlendShapeIndex = -1;
    private float blinkTimer = 0f;
    private float nextBlinkTime;
    private bool isBlinking = false;
    private float blinkProgress = 0f;

    void Start()
    {
        // Find blink blend shape index
        if (faceMesh != null && faceMesh.sharedMesh != null)
        {
            blinkBlendShapeIndex = faceMesh.sharedMesh.GetBlendShapeIndex(blinkBlendShapeName);
        }

        if (blinkBlendShapeIndex == -1)
        {
            Debug.LogError("Blink blend shape not found! Check name and mesh renderer.");
            enabled = false;
            return;
        }

        // Set initial random blink time
        ResetBlinkTimer();
    }

    void Update()
    {
        if (isBlinking)
        {
            HandleBlinkAnimation();
        }
        else
        {
            UpdateBlinkTimer();
        }
    }

    void UpdateBlinkTimer()
    {
        blinkTimer += Time.deltaTime;

        if (blinkTimer >= nextBlinkTime)
        {
            StartBlink();
        }
    }

    void StartBlink()
    {
        isBlinking = true;
        blinkProgress = 0f;
    }

    void HandleBlinkAnimation()
    {
        blinkProgress += Time.deltaTime / blinkDuration;

        // Apply bell curve for natural blink (smooth in/out)
        float blinkWeight = Mathf.Sin(blinkProgress * Mathf.PI);
        faceMesh.SetBlendShapeWeight(blinkBlendShapeIndex, blinkWeight * maxBlinkWeight);

        // Finish blink
        if (blinkProgress >= 1f)
        {
            isBlinking = false;
            ResetBlendShape();
            ResetBlinkTimer();
        }
    }

    void ResetBlinkTimer()
    {
        blinkTimer = 0f;
        nextBlinkTime = Random.Range(minBlinkInterval, maxBlinkInterval);
    }

    void ResetBlendShape()
    {
        faceMesh.SetBlendShapeWeight(blinkBlendShapeIndex, 0f);
    }

    // For manual blink triggering from other scripts
    public void TriggerBlink()
    {
        if (!isBlinking) StartBlink();
    }
}