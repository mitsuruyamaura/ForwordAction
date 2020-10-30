using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public void SetUpResultPopUp(int score) {
        canvasGroup.alpha = 0;

        canvasGroup.DOFade(1.0f, 1.0f);

        txtScore.text = score.ToString();
    }
}
