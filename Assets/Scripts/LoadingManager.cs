using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public ConfigurationSO configData;
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
        progressBarImage.fillAmount = 0.7f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        operation.allowSceneActivation = false;
        while(!operation.isDone)
        {
            progressBarImage.fillAmount = 0.7f + operation.progress*0.3f;
            yield return null;
        }
        operation.allowSceneActivation = true;
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
