using UnityEngine;

public class BombThrower : MonoBehaviour
{
    public GameObject bombPrefab; // Prefab of the bomb to instantiate
    public float throwForce = 3f; // Adjustable force to throw the bomb
    public float forwardOffset = 0.3f; // Offset distance in front of the player for the bomb spawn point

    private Transform handTransform; // Reference to the hand position
    private Animator animator;       // Animator to access the humanoid rig

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Find the right hand bone using the Humanoid Rig
            handTransform = animator.GetBoneTransform(HumanBodyBones.LeftHand);

            if (handTransform == null)
            {
                Debug.LogError("Right hand bone not found on the humanoid rig!");
            }
        }
        else
        {
            Debug.LogError("Animator component not found on this GameObject!");
        }
    }

    // This method will be triggered by the animation event
    public void DetachAndThrow()
    {
        if (handTransform == null)
        {
            Debug.LogError("Hand transform is not assigned!");
            return;
        }

        // Instantiate a new bomb at the hand's position
        GameObject bombInstance = Instantiate(bombPrefab, handTransform.position, Quaternion.identity);

        // Adjust the spawn position slightly in front of the player
        Vector3 throwPosition = handTransform.position - (handTransform.forward * forwardOffset);
        bombInstance.transform.position = throwPosition;

        // Apply a forward force to the bomb based on the player's direction
        Rigidbody rb = bombInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Ensure the bomb can move
            rb.AddForce(handTransform.forward * throwForce, ForceMode.Impulse);
        }

        // Optionally destroy the bomb after some time (e.g., simulate explosion)
        Destroy(bombInstance, 1.0f); // Adjust as needed
    }
}
