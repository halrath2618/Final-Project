using Pathfinding;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.velocity.magnitude > 0.1f)
        {
            animator.SetFloat("Speed", 5f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}
