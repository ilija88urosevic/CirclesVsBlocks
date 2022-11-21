using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public event EventHandler coinsUpdate;
    PlayerDataSO currentCoins;

    public int CurrentCoins { get { return currentCoins.Coins; } }

    internal void Init(PlayerDataSO data)
    {
        currentCoins = data;
        if (coinsUpdate != null)
            coinsUpdate(currentCoins.Coins, null);
        coinsText.text = currentCoins.Coins.ToString();
    }
    public void ChangeCoins(int amount)
    {
        currentCoins.Coins += amount;
        if (coinsUpdate != null)
            coinsUpdate(currentCoins.Coins, null);
        coinsText.text = currentCoins.Coins.ToString();
        
    }

    internal void Subscribe(EventHandler subscriber)
    {
        coinsUpdate += subscriber;
    }

}
