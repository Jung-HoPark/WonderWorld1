using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButton : MonoBehaviour
{
    public ActionData actionData;
    private BattleManager battleManager;
    private Image buttonImage;
    // public TextMeshProUGUI skillNameText;
    
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

        // 아이콘 교체
        if (buttonImage != null && newData.actionIcon != null)
        {
            buttonImage.sprite = newData.actionIcon;
        }

        // if (skillNameText != null) skillNameText.text = newData.actionName;
    }
}
