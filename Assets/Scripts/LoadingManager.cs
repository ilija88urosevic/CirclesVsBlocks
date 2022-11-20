using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Configuration
{
    internal void LoadData(string v)
    {
        throw new NotImplementedException();
    }

    internal string GetStringData()
    {
        throw new NotImplementedException();
    }
}

public class LoadingManager : MonoBehaviour
{
    public Configuration configData;
    public static string CONFIG_URL = @"https://raw.githubusercontent.com/ilija88urosevic/CirclesVsBlocks/main/Config.txt";
    [SerializeField]
    private Image progressBarImage;
    private IEnumerator Start()
    {
        if (PlayerPrefs.GetInt("FirstLoad", 0) == 0)
        {
            PlayerPrefs.SetString("ConfigData", configData.GetStringData());
            PlayerPrefs.GetInt("FirstLoad", 1);
            PlayerPrefs.Save();

        }
        yield return StartCoroutine(LoadConfig());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    private IEnumerator LoadConfig()
    {
        UnityWebRequest www = new UnityWebRequest(CONFIG_URL);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SendWebRequest();
        while (!www.isDone)
        {
            progressBarImage.fillAmount = www.downloadProgress * 0.5f;
            yield return null;
        }
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            //Continue game with existing config.
        }
        else if (www.isDone)
        {
            Debug.Log(www.downloadHandler.text);
            //Save For next loading.
            PlayerPrefs.SetString("ConfigData", www.downloadHandler.text);
        }
        configData.LoadData(PlayerPrefs.GetString("ConfigData"));


    }
}
