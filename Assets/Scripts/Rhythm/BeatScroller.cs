using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo; // Beats per minute
    public bool hasStarted; // Flag to check if the beat has started
    public float tempoMultiplier; // Multiplier for the beat tempo
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatTempo = beatTempo / 60f; // Convert beats per minute to beats per second
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            //if (Input.anyKeyDown)
            //{
            //    hasStarted = true; // Set the flag to true when any key is pressed
            //}
        }
        else
        {
            transform.position += new Vector3(0f, beatTempo * Time.deltaTime * tempoMultiplier, 0f); // Move downwards based on the beat tempo
        }
    }
}
