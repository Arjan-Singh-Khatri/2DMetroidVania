using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Data.Common;

public class BatDoTweenUI : MonoBehaviour
{
    [SerializeField] Vector3[] path;
    [SerializeField] float _tweenDuration;
    [SerializeField] PathType _pathType;
    [SerializeField] PathMode _pathMode;
    [SerializeField] LoopType _loopType;


    void Start(){
        transform.DOPath(path,_tweenDuration,_pathType,_pathMode).SetLoops(-1,_loopType);
    }

}
