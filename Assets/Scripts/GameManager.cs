using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    public PlayerDataSO playerData;
    public ConfigurationSO configuration;
    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    internal ConfigurationSO RequestConfig()
    {
        return configuration;
    }

    internal void LoadData(string data)
    {
        configuration.LoadData(data);
        playerData.LoadData(configuration.DefaultCoins,configuration.DefaultData);
    }

    internal PlayerDataSO RequestData()
    {
        return playerData;
    }
}
