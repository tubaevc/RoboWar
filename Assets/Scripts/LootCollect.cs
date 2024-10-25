using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootCollect : MonoBehaviour
{
    public float lootValue = 1f;
    private float totalLoot = 0f;
    [SerializeField] private Slider lootSlider;
    public float maxLoot = 5;
    [SerializeField] private TMP_Text level;
    public int levelCount;
    [SerializeField] private GameObject upgradePanel;
    public static event Action OnUpgradeAvailable;

    private void Start()
    {
        if (lootSlider != null)
        {
            lootSlider.maxValue = maxLoot;
            totalLoot = lootSlider.value;
        }

        level.text = levelCount.ToString();
     upgradePanel.SetActive(false);
    }

    private void Update()
    {
        LootUpgrade();
    }

    public void CollectLoot()
    {
        totalLoot += lootValue;
        if (lootSlider != null)
        {
            lootSlider.value = Mathf.Clamp(totalLoot, 0, maxLoot);
        }
    }

    private void LootUpgrade()
    {
        if (totalLoot >= maxLoot)
        {
            levelCount++;
            level.text = levelCount.ToString();

            totalLoot = 0;
            lootSlider.value = 0;

            if (levelCount == 2)
            {
                Debug.Log("level 2!");
                OnUpgradeAvailable?.Invoke();
                OpenUpgradePanel();
            }
        }
    }

    private void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("UpgradePanel not found!");
        }
    }
}