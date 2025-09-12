using System.Collections;
using UnityEngine;

public class DialogueBlendShapeController : MonoBehaviour
{
    [Header("Mouth Blend Shapes")]
    public string aaBlendShape = "vrc.v_aa"; // Your "AA" viseme blend shape
    public string ohBlendShape = "vrc.v_oh"; // Optional other visemes
    public string eeBlendShape = "vrc.v_e";

    [Header("Speech Settings")]
    public float visemeChangeSpeed = 10f;
    public float minVisemeHoldTime = 0.08f;
    public float maxVisemeHoldTime = 0.2f;
    public float baseAmplitude = 70f; // How much mouth opens (0-100)

    [SerializeField] private SkinnedMeshRenderer faceMesh;
    private int aaIndex, ohIndex, eeIndex;
    private float currentAAWeight;
    private Coroutine speechRoutine;
    private bool isTalking = false;

    void Awake()
    {

        // Get blend shape indices
        aaIndex = faceMesh.sharedMesh.GetBlendShapeIndex(aaBlendShape);
        ohIndex = faceMesh.sharedMesh.GetBlendShapeIndex(ohBlendShape);
        eeIndex = faceMesh.sharedMesh.GetBlendShapeIndex(eeBlendShape);

        if (aaIndex == -1) Debug.LogError($"Blend shape '{aaBlendShape}' not found!");
    }

    public void StartTalking()
    {
        if (isTalking) return;

        isTalking = true;
        speechRoutine = StartCoroutine(SpeechAnimation());
    }

    public void StopTalking()
    {
        if (!isTalking) return;

        isTalking = false;
        if (speechRoutine != null) StopCoroutine(speechRoutine);
        ResetAllVisemes();
    }

    IEnumerator SpeechAnimation()
    {
        while (isTalking)
        {
            // Randomly select a viseme (focus on AA but mix others)
            float visemeSelection = Random.value;
            int targetViseme;
            float targetWeight;

            if (visemeSelection < 0.6f) // 60% chance for AA
            {
                targetViseme = aaIndex;
                targetWeight = baseAmplitude * Random.Range(0.8f, 1.2f);
            }
            else if (visemeSelection < 0.8f) // 20% chance for OH
            {
                targetViseme = ohIndex;
                targetWeight = baseAmplitude * Random.Range(0.3f, 0.6f);
            }
            else // 20% chance for EE
            {
                targetViseme = eeIndex;
                targetWeight = baseAmplitude * Random.Range(0.2f, 0.5f);
            }

            // Smooth transition to target viseme
            float timer = 0f;
            float duration = Random.Range(minVisemeHoldTime, maxVisemeHoldTime);

            while (timer < duration && isTalking)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;

                // Reset all other visemes first
                ResetAllVisemes();

                // Animate current viseme
                faceMesh.SetBlendShapeWeight(targetViseme,
                    Mathf.Lerp(0, targetWeight, Mathf.Sin(progress * Mathf.PI)));

                yield return null;
            }

            // Brief pause between visemes
            if (isTalking) yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
        }

        ResetAllVisemes();
    }

    void ResetAllVisemes()
    {
        if (aaIndex != -1) faceMesh.SetBlendShapeWeight(aaIndex, 0);
        if (ohIndex != -1) faceMesh.SetBlendShapeWeight(ohIndex, 0);
        if (eeIndex != -1) faceMesh.SetBlendShapeWeight(eeIndex, 0);
    }

    void OnDisable()
    {
        StopTalking();
    }
}