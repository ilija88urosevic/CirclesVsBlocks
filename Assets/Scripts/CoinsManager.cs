using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public event EventHandler coinsUpdate;
    int currentCoins;

    public int CurrentCoins { get { return currentCoins; } }

    internal void SetCoins(int amount)
    {
        currentCoins = amount;
        if (coinsUpdate != null)
            coinsUpdate(currentCoins, null);
        coinsText.text = currentCoins.ToString();
    }
    public void ChangeCoins(int amount)
    {
        currentCoins += amount;
        if (coinsUpdate != null)
            coinsUpdate(currentCoins, null);
        coinsText.text = currentCoins.ToString();
        
    }

    internal void Subscribe(EventHandler subscriber)
    {
        coinsUpdate += subscriber;
    }

}
