using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleObjectController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleObj;

    public void MoveObject() {
        titleObj.transform.DOMoveX(15, 2.0f).SetEase(Ease.Linear);
    }
}
