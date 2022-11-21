using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int coins = 0;
    //List of upgrade levels, First is Player Tap, 0 means helper wasnt bought.
    public List<int> tapUpgradeLevels = new List<int> { 1, 0, 0, 0, 0 };

    public PlayerData(int coins, int maxCircles)
    {
        this.coins = coins;
        tapUpgradeLevels = new List<int>();
        tapUpgradeLevels.Add(1);
        for (int i = 0; i < maxCircles; i++)
            tapUpgradeLevels.Add(0);
    }
    public PlayerData(int coins, List<int> data)
    {
        this.coins = coins;
        tapUpgradeLevels = new List<int>();
        for (int i = 0; i < data.Count; i++)
            tapUpgradeLevels.Add(data[i]);
    }

    public PlayerData(int coins)
    {
        this.coins = coins;
        tapUpgradeLevels = new List<int>();
        tapUpgradeLevels.Add(1);
    }

}
[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 2)]
public class PlayerDataSO : ScriptableObject
{
    public PlayerData playerData;

    public int PlayerLevel => playerData.tapUpgradeLevels[0];
    public int CircleCount => playerData.tapUpgradeLevels.Count - 1;

    public List<int> CircleLevels => playerData.tapUpgradeLevels.AsEnumerable<int>().Skip(1).ToList<int>();

    public int Coins
    {
        get
        {
            return playerData.coins;
        }
        set
        {
            playerData.coins = value;
        }
    }

    internal void LoadData(int defualtCoins, string defaultData)
    {
        string data = SaveManager.GetString("PlayerData", defaultData);
        var dataArray = data.Split('#').Select(int.Parse).ToList();
        int coins = SaveManager.GetInt("coins", defualtCoins);
        playerData = new PlayerData(coins, dataArray);
    }

    public override string ToString()
    {
        if (playerData == null)
            playerData = new PlayerData(50);
        return string.Join("#", playerData.tapUpgradeLevels);
    }

    internal void Save()
    {
        SaveManager.SetString("PlayerData", ToString());
    }

    internal void SaveCoins()
    {
        SaveManager.SetInt("coins", Coins);
    }

    internal void LoadData(object defaultCoins, object defaultData)
    {
        throw new NotImplementedException();
    }

    internal void LoadData(int defaultCoins, object defaultData)
    {
        throw new NotImplementedException();
    }
}
