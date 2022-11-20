using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    public float coins = 0;
    //List of upgrade levels, First is Player Tap, 0 means helper wasnt bought.
    public List<int> tapUpgradeLevels =new List<int> {1,0,0,0,0};

    public PlayerData(int coins, int maxCircles)
    {
        this.coins = coins;
        tapUpgradeLevels = new List<int>();
        tapUpgradeLevels.Add(1);
        for(int i = 0; i< maxCircles; i++)
            tapUpgradeLevels.Add(0);
    }
}
[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 2)]
public class PlayerDataSO : ScriptableObject
{
    
}
