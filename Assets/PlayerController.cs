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

    public int maxBallonCount;

    public List<Ballon> ballonList = new List<Ballon>();

    public Transform[] ballonTrans;

    public Ballon ballonPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;

        // バルーン生成
        GenetateBallon(maxBallonCount);
    }

    private void GenetateBallon(int ballonCount) {
        // バルーンの最大値の場合にはバルーンを生成しない
        if (ballonList.Count >= maxBallonCount) {
            return;
        }

        for (int i = 0; i < maxBallonCount; i++) {
            // バルーン生成
            Ballon ballon = Instantiate(ballonPrefab, ballonTrans[i]);

            // バルーンの設定
            ballon.SetUpBallon(this);

            ballonList.Add(ballon);
        }
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

    /// <summary>
    /// バルーン破壊
    /// </summary>
    /// <param name="ballon"></param>
    public void DestroyBallon(Ballon ballon) {
        ballonList.Remove(ballon);
        Destroy(ballon.gameObject);
    }
}
