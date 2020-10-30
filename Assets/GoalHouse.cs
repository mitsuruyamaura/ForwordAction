using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHouse : MonoBehaviour
{
    public float moveSpeed;

    private float stopPos = 6.5f;

    void Update() {
        if (transform.position.x > stopPos) {
            transform.position += new Vector3(-moveSpeed, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();

        }
    }
}
