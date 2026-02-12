using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ActionData actionData;
    private BattleManager battleManager;
    private Image buttonImage;
    public ActionData assignedAction;
    public Image iconImage;
    public TextMeshProUGUI tooltipText;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        battleManager = FindObjectOfType<BattleManager>();
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }
    void OnButtonClick()
    {
        if (battleManager != null && actionData != null)
        {
            battleManager.SelectPlayerAction(actionData);
        }
    }
    public void SetAction(ActionData newData)
    {
        if (newData == null) return;

        actionData = newData;
        assignedAction = newData;

        // 아이콘 교체
        if (buttonImage != null && newData.actionIcon != null)
        {
            buttonImage.sprite = newData.actionIcon;
        }

        // if (skillNameText != null) skillNameText.text = newData.actionName;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (assignedAction != null && tooltipText != null)
        {
            tooltipText.text = $"{assignedAction.actionName}\n피해량: {assignedAction.damageValue}\n마나소모: {assignedAction.manaCost}";
            tooltipText.gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipText != null)
            tooltipText.gameObject.SetActive(false);
    }
}