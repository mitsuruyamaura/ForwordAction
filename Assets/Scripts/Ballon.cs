using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ballon : MonoBehaviour
{
    private PlayerController playerController;
    private Tweener tweener;

    private bool isDetached;
    private Rigidbody2D rb;
    private Vector2 pos;

    /// <summary>
    /// バルーンの初期設定
    /// </summary>
    /// <param name="playerController"></param>
    public void SetUpBallon(PlayerController playerController) {
        this.playerController = playerController;

        // 本来のScaleを保持
        Vector3 scale = transform.localScale;

        // 現在のScaleを0にして画面から一時的に非表示にする
        transform.localScale = Vector3.zero;

        // だんだんバルーンが膨らむアニメ演出
        transform.DOScale(scale, 2.0f).SetEase(Ease.InBounce).SetLink(gameObject);

        // 左右にふわふわさせる
        tweener = transform.DOLocalMoveX(0.02f, 0.2f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col) {

        if(col.gameObject.TryGetComponent(out VerticalFloatingObject enemy)) { 

        //if (col.gameObject.tag == "Enemy") {

            // PlayerControllerのDestroyBallonメソッドを呼び出し、バルーンの破壊処理を行う
            playerController.DestroyBallon(this);


            // 左右にふわふわループアニメを破棄する
            tweener.Kill();


            //playerController.DestroyBallon();
        }
    }

    /// <summary>
    /// バルーンを上空へ飛ばす準備
    /// </summary>
    public void FloatingBallon() {
        // 左右にふわふわループアニメを破棄する
        tweener.Kill();

        // Rigidbody2Dコンポーネントをバルーンに追加して代入
        rb = gameObject.AddComponent<Rigidbody2D>();

        // 重力は0にする
        rb.gravityScale = 0;

        // 回転も固定する
        rb.freezeRotation = true;

        // バルーンのコライダーを取得して、スイッチをオフにする
        GetComponent<CapsuleCollider2D>().enabled = false;

        // バルーンの位置情報を代入
        pos = transform.position;

        // 親子関係を解消する(特にPlayerが地面・床の子オブジェクトになっている場合に不具合になる)
        transform.SetParent(null);

        // バルーンとプレイヤーを切り離す状態にする
        isDetached = true;
    }

    void FixedUpdate() {

        // バルーンが切り離されていなければ、処理をしない
        if (isDetached == false) {
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
