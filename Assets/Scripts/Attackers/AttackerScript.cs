using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackerScript : MonoBehaviour
{
    public string header;
    public UpgradeView upgradeButton;
    private int upgradeLevel = 1;
    private Action<int,Transform> attackEvent;
    private ConfigurationSO config;
    private int attackValue;
    public void Init(int level, Action<EventHandler> coinsEvent, Action<int,Transform> attackAction,ConfigurationSO config )
    {
        attackEvent = attackAction;
        upgradeLevel = level;
        this.config = config;
        upgradeButton.SetUp(level, CoinsCost(), false, Upgrade, coinsEvent, header);
        attackValue = config.GetCoinsOnTap(upgradeLevel);
    }
    public void Upgrade()
    {
        int coinsToReduce = -CoinsCost();
        Debug.LogError("Upgrade " + coinsToReduce + gameObject.name, gameObject);
        upgradeLevel++;
        upgradeButton.Upgraded(upgradeLevel);
        upgradeButton.UpgradeCost(CoinsCost());
        attackEvent.Invoke(coinsToReduce, transform);
        attackValue = config.GetCoinsOnTap(upgradeLevel);
    }

    private int CoinsCost()
    {
        return config.GetUpgradeCost(upgradeLevel);
    }
   

    public virtual void Attack()
    {
        attackEvent.Invoke(attackValue, transform);
    }
}
