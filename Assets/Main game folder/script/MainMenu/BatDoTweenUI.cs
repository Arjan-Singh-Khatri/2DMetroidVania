using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Data.Common;

public class BatDoTweenUI : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] Vector3[] path;
    [SerializeField] float _tweenDuration;
    [SerializeField] PathType _pathType;
    [SerializeField] PathMode _pathMode;
    [SerializeField] LoopType _loopType;

    [SerializeField] Vector3 _rotation;
    [SerializeField] float rotateduration;
    Quaternion rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
    private bool loopedBack = false;
    void Start(){
        rectTransform = GetComponent<RectTransform>();
        transform.DOPath(path,_tweenDuration,_pathType,_pathMode).SetLoops(-1,_loopType);
    }
    private void Update()
    {
        //if (transform.position == path[7] || (loopedBack && transform.position == path[0])) {
        //    Debug.Log("LOOPED");
        //    loopedBack = true;
        //    rectTransform.rotation *= rotation;
        //}
    }
}
