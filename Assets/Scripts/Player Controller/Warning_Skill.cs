using System.Collections;
using UnityEngine;

public class Warning_Skill : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public IEnumerator Flashing()
    {
        float duration = 5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.PingPong(elapsedTime * 2, 1);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 0; // Ensure it ends fully transparent
    }
}
