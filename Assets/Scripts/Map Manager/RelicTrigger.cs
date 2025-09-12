using UnityEngine;

public class RelicTrigger : MonoBehaviour
{
    public GameObject relic;
    public GameObject player;
    public GameObject trigger;
    public GameObject cutscene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            // Activate the relic
            relic.SetActive(true);
            trigger.SetActive(false);
            cutscene.SetActive(true);
        }
    }
}
