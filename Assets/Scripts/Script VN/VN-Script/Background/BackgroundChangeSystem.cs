using UnityEngine;
using UnityEngine.UI;

public class BackgroundChangeSystem : MonoBehaviour
{
    public Image backgroundImage;

    // Call this method with the name of the new background sprite
    public void ChangeBackground(string newBackgroundName)
    {
        Sprite newBackground = Resources.Load<Sprite>(newBackgroundName);
        if (newBackground != null)
        {
            backgroundImage.sprite = newBackground;
        }
        else
        {
            Debug.LogError("Background image not found: " + newBackgroundName);
        }
    }
}
