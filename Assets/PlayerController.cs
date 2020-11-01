using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal";
    private string jump = "Jump";

    private Rigidbody2D rb;
    private Animator anim;

    public float jumpPower;
    public float moveSpeed;

    private float limitPosX = 9.5f;
    private float limitPosY = 4.35f;
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

    public bool isGameOver;

    public UIManager uiManager;

    void Start()
    {
        isGameOver = true;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        anim.SetBool("Idel", false);

        anim.SetTrigger("Generate");

        for (int i = 0; i < ballonCount; i++) {
            Ballon ballon;
            if (ballonList.Count == 0) {
                // 1つ目のバルーン生成
                ballon = Instantiate(ballonPrefab, ballonTrans[0]);
            } else {
                // 2つ目のバルーン生成
                ballon = Instantiate(ballonPrefab, ballonTrans[1]);
            }

            // バルーンの設定
            ballon.SetUpBallon(this);

            ballonList.Add(ballon);

            yield return new WaitForSeconds(waitTime);
        }

        // バルーンの数で重力を変化
        ChangeGravityScale();

        // 生成中状態終了。再度生成できるようにする
        isGenerating = false;

        anim.SetBool("Idel", true);
    }

    // Update is called once per frame
    void Update()
    {
        // 地面接地
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 1f, transform.position - transform.up * 0.8f, groundLayer);

        Debug.DrawLine(transform.position + transform.up * 1f, transform.position - transform.up * 0.8f, Color.red, 1.0f);

        // バルーンが１つ以上あるなら
        if (ballonList.Count > 0) {

            // ジャンプ。バルーンの数が少ないとジャンプの距離も少なくなる
            if (Input.GetButtonDown(jump)) {
                rb.AddForce(transform.up * jumpPower * ballonList.Count / maxBallonCount);
                anim.SetBool("Idel", false);
                anim.SetFloat("Run", 0);
                anim.SetTrigger("Jump");
            }

            // 空中にいる間にRボタンを押すと
            if (Input.GetKeyDown(KeyCode.R) && isGrounded == false) {
                // すべてのバルーンを切り離す(地面や床にいる間は不可)
                DetachBallons();
            }
        }

        // 一番高い場所まで到達している場合
        if(rb.velocity.y > 3.0f) {
            // Y軸の速度に制限をかける(そうしないと上空で待機できてしまう)
            rb.velocity = new Vector2(rb.velocity.x, 3.0f);
        }

        // 地面に接地していて、バルーンが２個以下の場合
        if (isGrounded == true && ballonList.Count < maxBallonCount) {

            // バルーンの生成中でなければ、バルーンを１つ作成する
            if (Input.GetKeyDown(KeyCode.Q) && isGenerating == false) {
                StartCoroutine(GenetateBallon(1, generateTime));
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

        // バルーンの数で重力を変化
        ChangeGravityScale();
    }

    void FixedUpdate() {
        // 移動
        Move();    
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move() {
        if (isGameOver == true) {
            
            return;
        }

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
            if (isGrounded == true) {
                anim.SetBool("Idel", false);
                anim.SetFloat("Run", rb.velocity.x);
            }
        } else {    //  左右の入力がなかったら横移動の速度を0にしてピタッと止まるようにする
            rb.velocity = new Vector2(0, rb.velocity.y);
            //  アニメの再生を止めてアイドル状態にする

            if (isGrounded == true) {
                anim.SetFloat("Run", 0.0f);
                anim.SetBool("Idel", true);
            }
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

        // バルーンの数で重力を変化
        ChangeGravityScale();
    }

    private void OnTriggerEnter2D(Collider2D col) {

        // コインに接触した場合
        if (col.gameObject.tag == "Coin") {
            coinCount += col.gameObject.GetComponent<Coin>().point;

            uiManager.UpdateDisplayScore(coinCount);
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
    /// ゲームオーバー
    /// </summary>
    public void GameOver() {
        isGameOver = true;
    }

    /// <summary>
    /// ゲームスタート
    /// </summary>
    public void GameStart() {
        isGameOver = false;
    }

    /// <summary>
    /// バルーンの数に応じて重力を変化
    /// </summary>
    private void ChangeGravityScale() {
        // バルーンの数が少ないほど重力が大きくなり、自然落下の速度が速くなる
        if (ballonList.Count == 0) {
            rb.gravityScale = 1.0f;
        } else if (ballonList.Count == 1){
            rb.gravityScale = 0.75f;
        } else {
            rb.gravityScale = 0.5f;
        }
    }
}
