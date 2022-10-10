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
            Debug.Log("RigidBody ���擾���܂����B");
        } else {
            Debug.Log("RigidBody ���擾�o���܂���ł����B");
        }

        hp = 100;
    }
}