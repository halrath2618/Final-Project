using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TailPhysics : MonoBehaviour
{
    [Header("Physics Settings")]
    public float tailDrag = 2f;
    public float tailAngularDrag = 5f;
    public float springPower = 100f;
    public float damper = 10f;

    private Rigidbody[] tailBones;
    private Vector3[] boneRestPositions;
    private Quaternion[] boneRestRotations;

    void Start()
    {
        // Get all tail bones (assign in Inspector or find automatically)
        tailBones = GetComponentsInChildren<Rigidbody>();

        // Store initial positions/rotations
        boneRestPositions = new Vector3[tailBones.Length];
        boneRestRotations = new Quaternion[tailBones.Length];

        for (int i = 0; i < tailBones.Length; i++)
        {
            boneRestPositions[i] = tailBones[i].transform.localPosition;
            boneRestRotations[i] = tailBones[i].transform.localRotation;

            // Configure physics
            tailBones[i].linearDamping = tailDrag;
            tailBones[i].angularDamping = tailAngularDrag;

            // Add spring joint if not exists
            if (!tailBones[i].GetComponent<SpringJoint>())
            {
                SpringJoint joint = tailBones[i].gameObject.AddComponent<SpringJoint>();
                joint.spring = springPower;
                joint.damper = damper;
                joint.enableCollision = true;

                // Connect to parent bone (or root if first bone)
                joint.connectedBody = (i == 0)
                    ? GetComponent<Rigidbody>()
                    : tailBones[i - 1];
            }
        }
    }

    void LateUpdate()
    {
        // Optional: Add subtle animation on top of physics
        for (int i = 0; i < tailBones.Length; i++)
        {
            if (i > 0) // Skip root bone
            {
                float wave = Mathf.Sin(Time.time * 2f + i * 0.5f) * 5f;
                tailBones[i].transform.Rotate(Vector3.right * wave * Time.deltaTime);
            }
        }
    }
}