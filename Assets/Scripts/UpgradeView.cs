using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI priceText;

    private int coinsCost;
    private Action onClick;

    public void SetUp(int level, int price,bool enabled, Action actionOnClick, EventHandler coinsEvent, string header)
    {
        coinsCost = price;
        levelText.text = "Level: " + level;
        priceText.text = coinsCost + "<sprite=0>";
        headerText.text = header;
        coinsEvent += CoinsUpdate;
        button.interactable = enabled;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(UpgradeClick);
        onClick = actionOnClick;
    }

    internal void UpgradeCost(int price)
    {
        coinsCost = price;
        priceText.text = coinsCost + "<sprite=0>";
    }

    private void UpgradeClick()
    {
        onClick.Invoke();
    }

    private void CoinsUpdate(object coins, EventArgs e)
    {
        button.interactable = (int)coins >= coinsCost;
    }
}
