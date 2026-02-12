using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public PlayerResourceManager resourceManager;

    [Header("HP Heart Settings")]
    public GameObject heartPrefab;
    public Transform heartContainer;
    private List<GameObject> hearts = new List<GameObject>();

    [Header("MP Bar Settings")]
    public Image mpBarFill;
    public TextMeshProUGUI mpText;

    [Header("EXP Settings")]
    public TextMeshProUGUI expText;

    void OnEnable()
    {
        resourceManager.OnResourceChange += UpdateUI;
        InitializeHearts();
        UpdateUI();
    }
    void OnDisable()
    {
        resourceManager.OnResourceChange -= UpdateUI;
    }
    void InitializeHearts()
    {
        foreach (Transform child in heartContainer) { Destroy(child.gameObject); }
        hearts.Clear();
        for (int i = 0; i < resourceManager.maxPlayerHeart; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < resourceManager.playerHeart) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }

        if (mpBarFill != null)
        {
            mpBarFill.fillAmount = resourceManager.playerMp / resourceManager.maxPlayerMp;
        }

        if (mpText != null)
        {
            mpText.text = $"{resourceManager.playerMp:F0} / {resourceManager.maxPlayerMp:F0}";
        }

        expText.text = $"EXP: {resourceManager.Exp}";
    }
}
