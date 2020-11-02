using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CanvasGroup canvasGroupRestart;

    [SerializeField]
    private Button btnTitle;

    public void SetUpResultPopUp(int score) {
        canvasGroup.alpha = 0;

        canvasGroup.DOFade(1.0f, 1.0f);

        txtScore.text = score.ToString();

        canvasGroupRestart.DOFade(0, 1.0f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

        btnTitle.onClick.AddListener(OnClickTitle);
    }

    /// <summary>
    /// ボタンを押した際の制御
    /// </summary>
    private void OnClickTitle() {

        // リザルト表示を徐々に非表示にする
        canvasGroup.DOFade(0, 1.0f).SetEase(Ease.Linear);

        // 現在のシーンを再度読み込む
        StartCoroutine(MoveTitle());
    }

    /// <summary>
    /// 現在のシーンを再度読み込む
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveTitle() {
        yield return new WaitForSeconds(1.0f);

        // 現在のシーンの名前を取得
        string sceneName = SceneManager.GetActiveScene().name;

        // 再度読み込み、タイトルから再スタート
        SceneManager.LoadScene(sceneName);
    }
}
