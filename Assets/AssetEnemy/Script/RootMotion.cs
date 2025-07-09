using UnityEngine;
using UnityEngine.AI;

public class RootMotion : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent parentAgent;
    private Vector3 childDeltaPosition;
    private Quaternion childDeltaRotation;
    private bool useRootMotion = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        parentAgent = GetComponentInParent<NavMeshAgent>();

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
    }

    // Gọi từ animation event hoặc script khác để bật/tắt root motion
    public void SetUseRootMotion(bool enable)
    {
        useRootMotion = enable;

        if (parentAgent != null)
        {
            parentAgent.updatePosition = !enable;
            parentAgent.updateRotation = !enable;
        }
    }

    void OnAnimatorMove()
    {
        if (animator == null || parentAgent == null || !useRootMotion) return;

        // Lấy delta movement từ animation
        childDeltaPosition = animator.deltaPosition;
        childDeltaRotation = animator.deltaRotation;

        // Áp dụng movement lên NavMeshAgent của object cha
        parentAgent.velocity = childDeltaPosition / Time.deltaTime;
        parentAgent.transform.rotation *= childDeltaRotation;
    }
}
