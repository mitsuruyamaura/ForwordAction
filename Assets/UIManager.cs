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

    public CanvasGroup canvasGroupInfo;

    public IEnumerator DisplayGameStartInfo() {
        canvasGroupInfo.alpha = 0;
        canvasGroupInfo.DOFade(1.0f, 0.5f);
        txtInfo.text = "Game Start!";

        yield return new WaitForSeconds(1.0f);
        canvasGroupInfo.DOFade(0f, 0.5f);
    }

    public void DisplayGameOverInfo() {

        canvasGroupInfo.DOFade(1.0f, 1.0f);
        txtInfo.text = "Game Over...";
    }

    public void UpdateDisplayScore(int score) {
        txtScore.text = score.ToString();
    }
}
