using System.Collections;
using UnityEngine;

public class SkillConstantlyActive : MonoBehaviour
{
    public GameObject skillEffect; // The effect to be activated
    public CanvasGroup canvasGroup; // The canvas group for the effect
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public IEnumerator SkillActively()
    {
        float duration = 5f; // Duration for the skill effect
        float elapsedTime = 1f;
        skillEffect.SetActive(true); // Activate the skill effect
        while (elapsedTime < duration * elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.PingPong(elapsedTime * 2, 1);
            canvasGroup.alpha = alpha; // Adjust the alpha of the canvas group
            yield return null; // Wait for the next frame
        }
        canvasGroup.alpha = 0; // Ensure it ends fully transparent
    }
}
