using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite; // The sprite when the button is not pressed
    public Sprite pressedSprite; // The sprite when the button is pressed

    public bool pressAble;

    public bool createMode; // Flag to indicate if the button is in create mode

    public GameObject UI;
    public GameManager gameManager; // Reference to the GameManager script

    public KeyCode keyToPress;
    public GameObject arrowPrefab; // Prefab for the arrow that can press the button
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sprite = normalSprite; // Set the initial sprite to normal sprite
    }

    // Update is called once per frame
    void Update()
    {
        if (createMode)
        {
            if (Input.GetKeyDown(keyToPress))
            {
                UI = Instantiate(arrowPrefab, transform.position, arrowPrefab.transform.rotation, transform); // Instantiate an arrow at the button's position
                gameManager.maxCombo++; // Increment the max combo in GameManager
            }
        }
        else
        {
            if (Input.GetKeyDown(keyToPress))
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
