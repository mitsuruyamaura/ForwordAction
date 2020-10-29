using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal";
    private string jump = "Jump";

    private Rigidbody2D rb;

    public float jumpPower;
    public float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプ
        if (Input.GetButtonDown(jump)) {
            rb.AddForce(transform.up * jumpPower);
        }

        // 移動

        float posX = Input.GetAxis(horizontal);

        rb.velocity = new Vector2(posX * moveSpeed, rb.velocity.y);

        // 移動方向に向きを合わせる
        if (posX > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        
    }
}
