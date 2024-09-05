using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FillButton : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    private bool _isFilling;
    public static Action onFilling;

    [SerializeField] private Image fill;
    public void OnPointerDown(PointerEventData eventData)
    {
        _isFilling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isFilling = false;
    }
    private void OnEnable()
    {
        Tower.onFillIncreased += OnFillIncreased;
    }

    private void OnFillIncreased(float fillAmount) => fill.fillAmount = fillAmount;
    private void OnDisable()
    {
        Tower.onFillIncreased -= OnFillIncreased;
    }

    private void Update()
    {
        if (_isFilling)
            onFilling?.Invoke();
    }

}
