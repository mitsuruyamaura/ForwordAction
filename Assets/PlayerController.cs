﻿using DG.Tweening;
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

    public float generateTime;

    public bool isGenerating;

    public int coinCount;

    public float knockbackPower;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;

        // バルーン生成
        StartCoroutine(GenetateBallon(maxBallonCount, 0));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ballonCount">生成するバルーンの数</param>
    /// <param name="waitTime">１つ生成する際の生成にかかる時間</param>
    /// <returns></returns>
    private IEnumerator GenetateBallon(int ballonCount, float waitTime) {
        // バルーンの最大値の場合にはバルーンを生成しない
        if (ballonList.Count >= maxBallonCount) {
            yield break;
        }

        // 生成中状態にする
        isGenerating = true;

        for (int i = 0; i < ballonCount; i++) {
            // バルーン生成
            Ballon ballon = Instantiate(ballonPrefab, ballonTrans[i]);

            // バルーンの設定
            ballon.SetUpBallon(this);

            ballonList.Add(ballon);

            yield return new WaitForSeconds(waitTime);
        }

        // 生成中状態終了。再度生成できるようにする
        isGenerating = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 地面接地
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 1f, transform.position - transform.up * 0.8f, groundLayer);

        Debug.DrawLine(transform.position + transform.up * 1f, transform.position - transform.up * 0.8f, Color.red, 1.0f);

        // バルーンが１つ以上あるなら
        if (ballonList.Count > 0) {

            // ジャンプ
            if (Input.GetButtonDown(jump)) {
                rb.AddForce(transform.up * jumpPower);
            }          
        }

        if(rb.velocity.y > 3.0f) {
            rb.velocity = new Vector2(rb.velocity.x, 3.0f);
        }

        // 地面に接地していて、バルーンが２個以下の場合
        if (isGrounded == true && ballonList.Count < maxBallonCount) {

            // バルーンの生成中でなければ、バルーンを１つ作成する
            if (Input.GetKeyDown(KeyCode.Q) && isGenerating == false) {
                StartCoroutine(GenetateBallon(maxBallonCount, generateTime));
            }
        }

        //　バルーンが１つ以上ある場合
        if (ballonList.Count > 0) {
            
            if (Input.GetKeyDown(KeyCode.R)) {
                // すべてのバルーンを切り離す
                DetachBallons();
            }
        }
    }

    /// <summary>
    /// すべてのバルーンを切り離す
    /// </summary>
    private void DetachBallons() {
        for (int i = 0; i < ballonList.Count; i++) {
            // バルーンを上空へ浮遊させる
            ballonList[i].FloatingBallon();
        }

        // バルーンのリストをクリアし、再度、バルーンを生成できるようにする
        ballonList.Clear();
    }

    void FixedUpdate() {
        // 移動
        Move();    
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move() {
        // 水平(横)方向への入力受付
        float x = Input.GetAxis(horizontal);

        if (x != 0) {
            // 移動
            rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

            // 移動方向に向きを合わせる
            Vector3 temp = transform.localScale;
            temp.x = x;

            //  向きが変わるときに少数になるとキャラが縮んでしまうので
            //  それを正の数にしてちゃんと描画する値にしてから戻す
            //  数字が0よりも大きければすべて1にする
            if (temp.x > 0) {
                temp.x = scale;
            } else {     //  数字が0よりも小さければすべて-1にする
                temp.x = -scale;
            }
            transform.localScale = temp;  //  数値を戻す             
                                          
            //  歩くアニメを再生する
            //anim.SetFloat("Run", 0.7f);
        } else {    //  左右の入力がなかったら横移動の速度を0にしてピタッと止まるようにする
            rb.velocity = new Vector2(0, rb.velocity.y);
            //  アニメの再生を止めてアイドル状態にする
            //anim.SetFloat("Run", 0.0f);
        }

        // 移動範囲の制限
        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        // 制限範囲を超えた場合、移動を制限する
        transform.position = new Vector2(posX, posY);
    }

    /// <summary>
    /// バルーン破壊
    /// </summary>
    /// <param name="ballon"></param>
    public void DestroyBallon(Ballon ballon) {
        ballonList.Remove(ballon);
        Destroy(ballon.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {

        // コインに接触した場合
        if (col.gameObject.tag == "Coin") {
            coinCount++;
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {

            // プレイヤーと敵の位置から距離と方向を計算
            Vector3 direction = (transform.position - col.transform.position).normalized;

            // 敵の反対側に吹き飛ばされる
            //transform.DOMove(transform.position += (direction * knockbackPower), 0.1f);
            transform.position += (direction * knockbackPower);
        }
    }

    /// <summary>
    /// ゴール到着
    /// </summary>
    public void Goal() {

    }
}
