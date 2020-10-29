using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float scrollSpeed = 0.01f;

    public float deadLine = -16f;

    public float startLine = 15.8f;

    void Start()
    {
        startLine = transform.position.x;
        Debug.Log(startLine);
    }

    void Update() {
        
        // 画面の左方向に背景を移動する
        transform.Translate(-scrollSpeed, 0, 0);

        if (transform.position.x < deadLine) {
            transform.position = new Vector2(startLine, 0);
        }
    }
}
