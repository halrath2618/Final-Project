using ECM.Components;
using UnityEngine;
namespace ECM.Movement
{
    public class Animation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterMovement characterMovement;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (characterMovement.forwardSpeed > 0.1f)
            {
                animator.SetFloat("Speed", characterMovement.forwardSpeed);
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}
