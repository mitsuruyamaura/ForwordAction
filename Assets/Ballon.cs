using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ballon : MonoBehaviour
{
    private PlayerController playerController;

    public bool isFloating;
    private Rigidbody2D rb;
    private Vector2 pos;

    //private SpriteRenderer spriteRenderer;


    public void SetUpBallon(PlayerController playerController) {
        this.playerController = playerController;
        //spriteRenderer = GetComponent<SpriteRenderer>();

        // 本来のScaleを保持
        Vector3 scale = transform.localScale;

        // 現在のScaleを0にして画面から一時的に非表示にする
        transform.localScale = Vector3.zero;

        // だんだんバルーンが膨らむアニメ演出
        transform.DOScale(scale, 2.0f).SetEase(Ease.InBounce);

        // 左右にふわふわさせる
        transform.DOLocalMoveX(0.05f, 0.2f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {

            // PlayerControllerのDestroyBallonメソッドを呼び出し、バルーンの破壊処理を行う
            playerController.DestroyBallon(this);
        }
    }

    /// <summary>
    /// バルーンを上空へ飛ばす準備
    /// </summary>
    public void FloatingBallon() {

        // Rigidbody2Dコンポーネントをバルーンに追加する
        rb = gameObject.AddComponent<Rigidbody2D>();

        // 重力は0にする
        rb.gravityScale = 0;

        // 回転も固定する
        rb.freezeRotation = true;

        // バルーンの位置情報を代入
        pos = transform.position;

        // バルーンとプレイヤーを切り離す状態にする
        isFloating = true;
    }

    void FixedUpdate() {

        // 切り離されていなければ、処理をしない
        if (isFloating == false) {
            return;
        }

        // バルーンの位置を上へ移動させる
        pos.y += 0.05f;

        // バルーンを左右に揺らす
        rb.MovePosition(new Vector2(pos.x + Mathf.PingPong(Time.time, 1.5f), pos.y));

        // 画面外にバルーンが出たら
        if (transform.position.y > 5.0f) {

            // 破壊する
            Destroy(gameObject);
        }

        //if (spriteRenderer.isVisible == false) {
        //    Destroy(gameObject);
        //}
    }


    //private void OnBecameInvisible() {
    //    Destroy(gameObject);   
    //}
}
