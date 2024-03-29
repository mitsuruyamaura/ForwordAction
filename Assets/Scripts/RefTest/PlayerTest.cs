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
            Debug.Log("RigidBody を取得しました。");
        } else {
            Debug.Log("RigidBody が取得出来ませんでした。");
        }

        hp = 100;
    }
}