using UnityEngine;
using UnityEngine.UI;

public class UIOpacity : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Scrollbar opacityScrollbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        opacityScrollbar.value = canvasGroup.alpha;
    }
}
