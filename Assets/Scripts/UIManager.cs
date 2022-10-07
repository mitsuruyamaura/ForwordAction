using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private CanvasGroup canvasGroupInfo;

    [SerializeField]
    private ResultPopUp resultPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private Button btnInfo;

    [SerializeField]
    private Button btnTitle;

    [SerializeField]
    private Text lblStart;

    [SerializeField]
    private CanvasGroup canvasGroupTitle;

    Tweener tweener;

    [Header("タイトル画面でクリックされたかどうか判定")]
    public bool isTitleClicked = false;

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    public void DisplayGameOverInfo() {

        canvasGroupInfo.DOFade(1.0f, 1.0f);

        txtInfo.DOText("Game Over...", 1.0f);

        btnInfo.onClick.AddListener(RestartGame);
    }

    /// <summary>
    /// スコア表示を更新
    /// </summary>
    /// <param name="score"></param>
    public void UpdateDisplayScore(int score) {
        txtScore.text = score.ToString();
    }

    /// <summary>
    /// ResultPopUpを生成
    /// </summary>
    /// <param name="score"></param>
    public void GenerateRusultPopUp(int score) {
        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);
        resultPopUp.SetUpResultPopUp(score);
    }

    /// <summary>
    /// タイトルへ戻る
    /// </summary>
    public void RestartGame() {
        Debug.Log("Restart");

        // ボタンからメソッドを削除(重複クリック防止)
        btnInfo.onClick.RemoveAllListeners();

        // 現在のシーンの名前を取得
        string sceneName = SceneManager.GetActiveScene().name;

        canvasGroupInfo.DOFade(0f, 1.0f).SetLink(gameObject).OnComplete(() => { SceneManager.LoadScene(sceneName); });
    }


    private void Start() {

        // タイトル表示
        SwitchDisplayTitle(true, 1.0f);

        btnTitle.onClick.AddListener(OnClickTitle);
    }

    /// <summary>
    /// タイトル表示
    /// </summary>
    public void SwitchDisplayTitle(bool isSwitch, float alpha) {
        if (isSwitch) canvasGroupTitle.alpha = 0;

        canvasGroupTitle.DOFade(alpha, 1.0f).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() => {
            lblStart.gameObject.SetActive(isSwitch);
        });

        // Tap Startの文字をゆっくり点滅させる
        tweener = lblStart.gameObject.GetComponent<CanvasGroup>().DOFade(0, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
            //.OnStepComplete(() => { Debug.Log("t"); });
        

    }

    /// <summary>
    /// タイトル表示中に画面をクリックした際の処理
    /// </summary>
    private void OnClickTitle() {
        // ボタンのメソッドを削除して重複タップ防止
        btnTitle.onClick.RemoveAllListeners();

        // タイトルを徐々に非表示
        SwitchDisplayTitle(false, 0.0f);

        // タイトル表示が消えるのと入れ替わりで、ゲームスタートの文字を表示する
        StartCoroutine(DisplayGameStartInfo());

        isTitleClicked = true;
    }

    /// <summary>
    /// ゲームスタート表示
    /// </summary>
    /// <returns></returns>
    public IEnumerator DisplayGameStartInfo() {
        yield return new WaitForSeconds(0.5f);

        canvasGroupInfo.alpha = 0;
        canvasGroupInfo.DOFade(1.0f, 0.5f).SetLink(gameObject);
        txtInfo.text = "Game Start!";

        yield return new WaitForSeconds(1.0f);
        canvasGroupInfo.DOFade(0f, 0.5f).SetLink(gameObject);

        canvasGroupTitle.gameObject.SetActive(false);
    }
}
