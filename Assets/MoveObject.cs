using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;

    void Update() {
        transform.position += new Vector3(-moveSpeed, 0, 0);

        // 画面に移らない位置まで移動したら
        if (transform.position.x <= -14.0f) {
            // このゲームオブジェクトがアタッチされているゲームオブジェクトを破壊
            Destroy(gameObject);
        }
    }
}
