using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterIntent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip UI")]
    public GameObject tooltipObject;
    private TextMeshProUGUI actualText;

    public ActionData currentIntentAction;
    public TextMeshProUGUI tooltipText;

    void Start()
    {
        if (tooltipObject != null)
        {
            actualText = tooltipObject.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentIntentAction != null && actualText != null)
        {
            actualText.text = $"{currentIntentAction.actionName}\n피해량: {currentIntentAction.damageValue}";
            tooltipObject.SetActive(true);
            Debug.Log($"툴팁 출력: {actualText.text}"); // 로그로 확인!
        }
    }
    public void OnPointerStay(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            // 마우스 위치로 툴팁 이동
            tooltipText.transform.position = Input.mousePosition + new Vector3(100, -50, 0);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipText.gameObject.SetActive(false);
    }
}
