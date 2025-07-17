using UnityEngine;

public class RootMotionToMonster : MonoBehaviour
{
    public Transform monsterRoot; // object cha = Monster
    public Animator animator;
    public bool applyRootMotion = false;

    private void Start()
    {
        animator.applyRootMotion = true; // bắt buộc
    }

    private void OnAnimatorMove()
    {
        if (!applyRootMotion || animator == null) return;

        Vector3 delta = animator.deltaPosition;
        Quaternion deltaRot = animator.deltaRotation;

        // Di chuyển enemy cha
        monsterRoot.position += delta;
        monsterRoot.rotation *= deltaRot;
    }
}
