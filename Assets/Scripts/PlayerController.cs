using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal";
    private string jump = "Jump";

    private Rigidbody2D rb;
    private Animator anim;

    private float limitPosX = 9.5f;
    private float limitPosY = 4.5f;

    public bool isFirstGenerateBallon;   // publicにする

    private bool isGameOver;

    public float moveSpeed;
    public float jumpPower;

    public bool isGrounded;

    //public GameObject[] ballons;

    public List<Ballon> ballonList = new List<Ballon>();

    public int maxBallonCount;

    public Transform[] ballonTrans;

    //public GameObject ballonPrefab;

    public Ballon BallonPrefab;

    public float generateTime;

    public bool isGenerating;

    public int coinPoint;

    public UIManager uiManager;


    [SerializeField, Header("Linecast用 地面判定レイヤー")]
    private LayerMask groundLayer;

    [SerializeField]
    private StartChecker startChecker;


    [SerializeField]
    private AudioClip knockbackSE;

    [SerializeField, HideInInspector]
    private AudioClip coinSE;

    [SerializeField]
    private GameObject knockbackEffectPrefab;

    [SerializeField, HideInInspector]
    private GameObject coinEffectPrefab;

    [SerializeField]
    private Joystick joystick;

    [SerializeField]
    private Button btnJump;

    [SerializeField]
    private Button btnDetach;

    private int ballonCount;


    // 未
    private float scale;

    public float knockbackPower;


    void Start()
    {
        isGameOver = false;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        scale = transform.localScale.x;

        btnJump.onClick.AddListener(OnClickJump);
        btnDetach.onClick.AddListener(OnClickDetachOrGenerate);

        // バルーン生成
        //StartCoroutine(GenetateBallon(maxBallonCount, 0));

        // 配列の初期化(位置の数だけ配列の要素数を用意する)
        //ballons = new GameObject[maxBallonCount];
    }

    /// <summary>
    /// バルーン生成
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
        //anim.SetBool("Idle", false);

        //anim.SetTrigger("Generate");

        // まだ１回もバルーンを生成していないなら
        if (isFirstGenerateBallon == false) {
            // 初回のバルーン生成済にする
            isFirstGenerateBallon = true;

            // StartChecker スクリプトが代入されている startChecker 変数を利用して、SetInitialSpeed メソッドを実行する
            startChecker.SetInitialSpeed();
        }


        for (int i = 0; i < ballonCount; i++) {
            Ballon ballon;

            // 1つ目のバルーンの生成位置にバルーンがない場合
            if (ballonTrans[0].childCount == 0) {
                // 1つ目のバルーン生成
                ballon = Instantiate(BallonPrefab, ballonTrans[0]);

                // 1つ目のバルーンの生成位置にバルーンがある場合
            } else {
                // 2つ目のバルーン生成
                ballon = Instantiate(BallonPrefab, ballonTrans[1]);
            }

            //if (ballonList.Count == 0) {
            //    // 1つ目のバルーン生成
            //    ballon = Instantiate(BallonPrefab, ballonTrans[0]);
            //} else {
            //    // 2つ目のバルーン生成
            //    ballon = Instantiate(BallonPrefab, ballonTrans[1]);
            //}

            // バルーンの設定
            ballon.SetUpBallon(this);

            ballonList.Add(ballon);

            yield return new WaitForSeconds(waitTime);
        }

        // バルーンの数で重力を変化
        ChangeLinearDrag();

        // 生成中状態終了。再度生成できるようにする
        isGenerating = false;

        //anim.SetBool("Idle", true);
    }

    /// <summary>
    /// バルーン生成
    /// </summary>
    /// <returns></returns>
    //private IEnumerator GenerateBallon() {
    //    // すべての配列にバルーンの最大値の場合にはバルーンを生成しない
    //    if (ballons[1] != null) {
    //        yield break;
    //    }

    //    // 生成中状態にする
    //    isGenerating = true;

    //    // ゲームを開始してから、まだバルーンを生成していないなら
    //    if (isFirstGenerateBallon == false) {
            
    //        // 初回バルーン生成終了とする
    //        isFirstGenerateBallon = true;

    //        Debug.Log("初回のバルーン生成");

    //        startChecker.SetInitialSpeed();
    //    }
        

    //    if (ballons[0] == null) {
    //        // 1つ目のバルーン生成
    //        ballons[0] = Instantiate(ballonPrefab, ballonTrans[0]);
    //        ballons[0].GetComponent<Ballon>().SetUpBallon(this);
    //    } else {
    //        // 2つ目のバルーン生成
    //        ballons[1] = Instantiate(ballonPrefab, ballonTrans[1]);
    //        ballons[1].GetComponent<Ballon>().SetUpBallon(this);
    //    }
    //    ballonCount++;

    //    // 生成時間分待機
    //    yield return new WaitForSeconds(generateTime);

    //    // 生成中状態終了。再度生成できるようにする
    //    isGenerating = false;
    //}


    void Update()
    {
        // タイトルクリックされるまで、Update を処理しない
        if (uiManager.isTitleClicked == false) {
            return;
        }

        // 地面接地  Physics2D.Linecastメソッドを実行して、Ground Layerとキャラのコライダーとが接地している距離かどうかを確認し、接地しているなら true、接地していないなら false を戻す
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, groundLayer);

        // Sceneビューに Physics2D.LinecastメソッドのLineを表示する
        //Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, Color.red, 1.0f);

        // バルーンが１つ以上あるなら
        if (ballonList.Count > 0) {　　　//  ballons[0] != null

            // ジャンプ。バルーンの数が少ないとジャンプの距離も少なくなる
            if (Input.GetButtonDown(jump)) {
                Jump();
            }

            // 空中にいる間にRボタンを押すと
            if (Input.GetKeyDown(KeyCode.R) && isGrounded == false) {
                // すべてのバルーンを切り離す(地面や床にいる間は不可)
                DetachBallons();
            }

            // 接地していない(空中にいる)間で、落下中の場合
            if (isGrounded == false && rb.velocity.y < 0.15f) {
                // 落下アニメを繰り返す
                anim.SetTrigger("Fall");
            }
        } else {
            Debug.Log("バルーンがない。ジャンプ不可");
        }

        // Velocity.y の値が 3.0f を超える場合(ジャンプを連続で押した回数が一定回数を超えた場合)
        if (rb.velocity.y > 5.0f) {
            // Velocity.y の値に制限をかける(落下せずに上空で待機できてしまう現象を防ぐため)
            rb.velocity = new Vector2(rb.velocity.x, 5.0f);
        }

        // 地面に接地していて、バルーンが２個以下の場合
        if (isGrounded == true && isGenerating == false && ballonList.Count < maxBallonCount) {　　　//ballonList.Count < maxBallonCount

            // バルーンの生成中でなければ、バルーンを１つ作成する
            if (Input.GetKeyDown(KeyCode.Q)) {
                StartCoroutine(GenetateBallon(1, generateTime));
                //StartCoroutine(GenerateBallon());
            }
        }
    }

    /// <summary>
    /// ジャンプと空中浮遊
    /// </summary>
    private void Jump() {
        // キャラの位置を上方向へ移動させる(ジャンプ・浮遊)
        rb.AddForce(transform.up * jumpPower); // * ballonList.Count / maxBallonCount);
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
        //    anim.SetBool("Idel", false);
        //    anim.SetFloat("Run", 0);
        //    //anim.SetTrigger("Jump");

        //}
        //anim.ResetTrigger("Fall");

        // Jump(Up + Mid) アニメーションを再生する
        anim.SetTrigger("Jump");
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
        ChangeLinearDrag();
    }

    void FixedUpdate() {
        // タイトルクリックされるまで、FixedUpdate を処理しない
        if (uiManager.isTitleClicked == false) {
            return;
        }

        // タイトルクリック後は、最初の床でバルーンを生成していないくても移動はすぐに出来るようにする

        // 移動
        Move();    
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move() {
        if (isGameOver == true) {  // これを入れておくと、ゲームオーバーゾーン通過時に、キャラの画面制限を超えて画面外下まで落下できる
            return;
        }

#if UNITY_EDITOR
        // 水平(横)方向への入力受付
        float x = joystick.Horizontal;   // この順番で書かないと JoyStick の方が優先されてしまって、キーボード入力値が 0 になってしまう
        x = Input.GetAxis(horizontal);
#else
        x = joystick.Horizontal;
        float x = joystick.Horizontal;
#endif
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

            //    //  歩くアニメを再生する
            //if (isGrounded == true) {
                anim.SetBool("Idle", false);
                anim.SetFloat("Run", 0.5f);
            //}
        } else {    //  左右の入力がなかったら横移動の速度を0にしてピタッと止まるようにする
            rb.velocity = new Vector2(0, rb.velocity.y);
            //  アニメの再生を止めてアイドル状態にする

            //if (isGrounded == true) {
                anim.SetFloat("Run", 0.0f);
                anim.SetBool("Idle", true);
                //    }
        }

        // 現在の位置情報が移動範囲の制限範囲を超えていないか確認する。超えていたら、制限範囲内に収める
        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        // 現在の位置を更新(制限範囲を超えた場合、ここで移動の範囲を制限する)
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
        ChangeLinearDrag();
    }

    /// <summary>
    /// バルーン破壊
    /// </summary>
    //public void DestroyBallon() {

    //    // TODO 後程、バルーンが破壊される際に「割れた」ように見えるアニメ演出を追加する
        
    //    if (ballons[1] != null) {
    //        Destroy(ballons[1]);
    //    } else if (ballons[0] != null){
    //        Destroy(ballons[0]);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D col) {

        if (col.TryGetComponent(out Coin coin)) { 
        //// コインに接触した場合
        //if (col.gameObject.tag == "Coin") {
            //coinPoint += col.gameObject.GetComponent<Coin>().point;
            coinPoint += coin.point;

            uiManager.UpdateDisplayScore(coinPoint);
            Destroy(col.gameObject);

            AudioSource.PlayClipAtPoint(coinSE, transform.position);

            GameObject coinEffect = Instantiate(coinEffectPrefab, col.transform.position, Quaternion.identity);
            Destroy(coinEffect, 0.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.TryGetComponent(out VerticalFloatingObject enemy)) { 
        //if (col.gameObject.tag == "Enemy") {

            // プレイヤーと敵の位置から距離と方向を計算
            Vector3 direction = (transform.position - col.transform.position).normalized;

            // 敵の反対側に吹き飛ばされる
            //transform.DOMove(transform.position += (direction * knockbackPower), 0.1f);
            transform.position += direction * knockbackPower;

            AudioSource.PlayClipAtPoint(knockbackSE, transform.position);

            GameObject knockbackEffect = Instantiate(knockbackEffectPrefab, col.transform.position, Quaternion.identity);
            Destroy(knockbackEffect, 0.5f);
        }
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver() {
        isGameOver = true;

        Debug.Log(isGameOver);

        uiManager.DisplayGameOverInfo();
    }

    /// <summary>
    /// ゲームスタート
    /// </summary>
    public void GameStart() {
        isGameOver = false;
    }

    /// <summary>
    /// バルーンの数に応じて空気抵抗を変化
    /// </summary>
    private void ChangeLinearDrag() {
        // バルーンの数が少ないほど空気抵抗が少なくなり、自然落下の速度が速くなる
        if (ballonList.Count == 0) {
            //rb.gravityScale = 1.0f;
            rb.drag = 0;
        } else if (ballonList.Count == 1){
            //rb.gravityScale = 0.75f;
            rb.drag = 1.5f;
        } else {
            //rb.gravityScale = 0.5f;
            rb.drag = 3f;
        }
    }

    /// <summary>
    /// ジャンプボタンを押した際の処理
    /// </summary>
    private void OnClickJump() {
        // バルーンが１つ以上あるなら
        if (ballonList.Count > 0) {
            Jump();
        }
    }

    /// <summary>
    /// バルーン生成ボタンを押した際の処理
    /// </summary>
    private void OnClickDetachOrGenerate() {
        // バルーンが1つ以上あり、空中にいる間なら
        if (ballonList.Count > 0 && !isGrounded) {
            // バルーンを切り離す
            DetachBallons();
        } else if (isGrounded == true && ballonList.Count < maxBallonCount && isGenerating == false) {
            // 地面に接地していて、バルーンが２個以下の場合
            // バルーンの生成中でなければ、バルーンを１つ作成する
            StartCoroutine(GenetateBallon(1, generateTime));
        }
    }
}
