using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTest : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private Rigidbody rb;

    void Reset() {
        if (TryGetComponent(out rb)) {
            Debug.Log("RigidBody ‚ğæ“¾‚µ‚Ü‚µ‚½B");
        } else {
            Debug.Log("RigidBody ‚ªæ“¾o—ˆ‚Ü‚¹‚ñ‚Å‚µ‚½B");
        }

        hp = 100;
    }
}