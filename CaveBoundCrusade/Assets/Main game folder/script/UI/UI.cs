using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button buttonTick;
    [SerializeField] private CanvasGroup fadeImage;
    [SerializeField] private RectTransform tickRect;
    [SerializeField] Ease easeType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            MoveUi();
        }

    }


    private void MoveUi()
    {
        fadeImage.DOFade(0, 1f);
        tickRect.DOAnchorPos(new Vector3(-190, 0, 0), 2f, false).SetEase(easeType);
        ;
    }

    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {

    }

}
