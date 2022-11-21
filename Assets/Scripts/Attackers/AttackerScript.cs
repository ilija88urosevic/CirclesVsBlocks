using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackerScript : MonoBehaviour
{
    private int id = 1;
    public string header;
    public UpgradeView upgradeButton;
    private PlayerDataSO playerData;
    private Action<int, Transform> attackEvent;
    private ConfigurationSO config;
    private int attackValue;
    public void Init(PlayerDataSO data, Action<EventHandler> coinsEvent, Action<int, Transform> attackAction, ConfigurationSO config, int id)
    {

        this.id = id;
        attackEvent = attackAction;
        playerData = data;
        this.config = config;
        upgradeButton.SetUp(playerData.playerData.tapUpgradeLevels[id], CoinsCost(), false, Upgrade, coinsEvent, header);
        attackValue = config.GetCoinsOnTap(playerData.playerData.tapUpgradeLevels[id]);
    }
    public void Upgrade()
    {
        int coinsToReduce = -CoinsCost();
        playerData.playerData.tapUpgradeLevels[id]++;
        upgradeButton.Upgraded(playerData.playerData.tapUpgradeLevels[id]);
        upgradeButton.UpgradeCost(CoinsCost());
        attackEvent.Invoke(coinsToReduce, transform);
        attackValue = config.GetCoinsOnTap(playerData.playerData.tapUpgradeLevels[id]);
        playerData.Save();
    }

    private int CoinsCost()
    {
        return config.GetUpgradeCost(playerData.playerData.tapUpgradeLevels[id]);
    }


    public virtual void Attack()
    {
        attackEvent.Invoke(attackValue, transform);
    }
}
