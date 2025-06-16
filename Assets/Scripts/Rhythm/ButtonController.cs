using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite; // The sprite when the button is not pressed
    public Sprite pressedSprite; // The sprite when the button is pressed

    public bool pressAble;

    public KeyCode keyToPress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sprite = normalSprite; // Set the initial sprite to normal sprite
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            spriteRenderer.sprite = pressedSprite; // Change to pressed sprite
            Debug.Log("Button Pressed: " + keyToPress);
        }
        else if (Input.GetKeyUp(keyToPress))
        {
            spriteRenderer.sprite = normalSprite; // Change back to normal sprite
            Debug.Log("Button Released: " + keyToPress);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            pressAble = true; // Set the button as pressable when an arrow enters the trigger
            // Logic for when the button is pressed by an arrow
            Debug.Log("Button hit by arrow!");
            Destroy(other.gameObject); // Destroy the arrow after it has hit the button
        }
    }
}
