using System.Linq;
using UnityEngine;

public class AnimationTransitionHelper : MonoBehaviour
{
    private Animator animator;
    private CharacterClassManager classManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        classManager = GetComponent<CharacterClassManager>();
    }

    public void OnClassChanged()
    {
        // Preserve current animation state
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = currentState.normalizedTime;

        // Rebind animator to apply new controller
        animator.Rebind();

        // Force animator to update immediately
        animator.Update(0f);

        // Restore previous state
        animator.Play(currentState.fullPathHash, 0, normalizedTime);

        // Optional: Sync any special parameters
        SyncAnimatorParameters();
    }

    private void SyncAnimatorParameters()
    {
        // Example: Sync movement speed
        if (animator.parameters.Any(p => p.name == "Speed"))
        {
            // Preserve current speed value
            float currentSpeed = animator.GetFloat("Speed");
            animator.SetFloat("Speed", currentSpeed);
        }
    }
}