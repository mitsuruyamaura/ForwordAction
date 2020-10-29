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

    private float limitPosX = 9.5f;
    private float limitPosY = 4.2f;
    private float scale;

    [SerializeField, Header("Linecast用 地面判定レイヤー")]
    private LayerMask groundLayer;

    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプ
        if (Input.GetButtonDown(jump)) {
            rb.AddForce(transform.up * jumpPower);
        }

        // 地面に接地したら、Y軸のVelocityを0にする(そうしないとずっとY軸のVelocityがマイナス方向に動く)

        isGrounded = Physics2D.Linecast(transform.position + transform.up * 1f, transform.position - transform.up * 0.3f, groundLayer);
    }

    void FixedUpdate() {
        // 移動
        Move();    
    }

    private void Move() {
        float posX = Input.GetAxis(horizontal);

        rb.velocity = new Vector2(posX * moveSpeed, rb.velocity.y);

        // 移動方向に向きを合わせる
        if (posX > 0) {
            transform.localScale = new Vector3(scale, scale, scale);
        } else {
            transform.localScale = new Vector3(-scale, scale, scale);
        }

        // 移動範囲の制限
        float x = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float y = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        transform.position = new Vector2(x, y);
    }
}
