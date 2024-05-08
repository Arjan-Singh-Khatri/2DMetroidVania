using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlimeUIDoTween : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] Vector3 _endPoint;
    [SerializeField] float _jumpPower;
    [SerializeField] int _jumpNumber;
    [SerializeField] float _jumpDuration;

    private void Start(){
        rectTransform = GetComponent<RectTransform>();
        SlimeJumpBackAndForth();
    }
    // Update is called once per frame
    void SlimeJumpBackAndForth(){
        rectTransform.DOJumpAnchorPos(_endPoint, _jumpPower,
            _jumpNumber, _jumpDuration).SetLoops(-1, LoopType.Yoyo);

    }
}
