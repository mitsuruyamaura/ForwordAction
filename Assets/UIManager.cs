using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private Text txtStart;

    public CanvasGroup canvasGroupInfo;

    [SerializeField]
    private CanvasGroup canvasGroupTitle;

    /// <summary>
    /// ゲームスタート表示
    /// </summary>
    /// <returns></returns>
    public IEnumerator DisplayGameStartInfo() {
        yield return new WaitForSeconds(0.5f);

        canvasGroupInfo.alpha = 0;
        canvasGroupInfo.DOFade(1.0f, 0.5f);
        txtInfo.text = "Game Start!";

        yield return new WaitForSeconds(1.0f);
        canvasGroupInfo.DOFade(0f, 0.5f);
    }

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    public void DisplayGameOverInfo() {

        canvasGroupInfo.DOFade(1.0f, 1.0f);
        txtInfo.text = "Game Over...";
    }

    /// <summary>
    /// スコア表示を更新
    /// </summary>
    /// <param name="score"></param>
    public void UpdateDisplayScore(int score) {
        txtScore.text = score.ToString();
    }

    /// <summary>
    /// タイトル表示
    /// </summary>
    public void SwitchDisplayTitle(bool isSwitch, float alpha) {
        if (isSwitch) canvasGroupTitle.alpha = 0;

        canvasGroupTitle.DOFade(alpha, 1.0f).SetEase(Ease.Linear).OnComplete(() => {
            txtStart.gameObject.SetActive(isSwitch);
        });

        // Tap Startの文字をゆっくり点滅させる
        txtStart.gameObject.GetComponent<CanvasGroup>().DOFade(0, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
