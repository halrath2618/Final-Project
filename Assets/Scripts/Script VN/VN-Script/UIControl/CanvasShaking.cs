using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasShaking : MonoBehaviour
{
    public RectTransform canvasTransform;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 10f;

    private Vector3 originalPos;

    void Start()
    {
        if (canvasTransform == null)
        {
            canvasTransform = GetComponent<RectTransform>();
        }
        originalPos = canvasTransform.localPosition;
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            canvasTransform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        canvasTransform.localPosition = originalPos;
    }
}
