using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float moveSpeed;

    void Update() {
        transform.position += new Vector3(-moveSpeed, 0, 0);
    }
}
