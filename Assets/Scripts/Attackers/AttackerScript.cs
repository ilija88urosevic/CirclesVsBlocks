using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackerScript : MonoBehaviour
{
    public string header;
    public UpgradeView upgradeButton;
    private int upgradeLevel = 1;
    Action<int,Transform> changeCoins;
    public void Init(int level, EventHandler coinsEvent, Action<int,Transform> usedCoins)
    {
        changeCoins = usedCoins;
        upgradeLevel = level;
        upgradeButton.SetUp(level, CoinsCost(), false, Upgrade, coinsEvent, header);
    }
    public void Upgrade()
    {
        int coinsToReduce = -CoinsCost();
        upgradeLevel++;
        upgradeButton.UpgradeCost(CoinsCost());
        changeCoins.Invoke(coinsToReduce,transform);
    }

    private int CoinsCost()
    {
        return 50;
    }
    private int AttackValue()
    {
        return 1;
    }

    public virtual void Attack()
    {
        changeCoins.Invoke(AttackValue(), transform);
    }
}
