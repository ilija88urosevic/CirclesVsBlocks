using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string CONFIG_URL = @"https://raw.githubusercontent.com/ilija88urosevic/CirclesVsBlocks/development/Config.txt";
    [SerializeField]
    private Image progressBarImage;
    private IEnumerator Start()
    {
      
        if (SaveManager.GetInt("FirstLoad", 0) == 0)
        {
            SaveManager.SetString("ConfigData", GameManager.Instance.configuration.GetStringData());
            SaveManager.GetInt("FirstLoad", 1);

        }
        yield return StartCoroutine(LoadConfig());
        yield return new WaitForSeconds(1);
        progressBarImage.fillAmount = 0.7f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        while(!operation.isDone)
        {
            progressBarImage.fillAmount = 0.7f + operation.progress*0.3f;
            yield return null;
        }
    }

    private IEnumerator LoadConfig()
    {
        UnityWebRequest www = new UnityWebRequest(CONFIG_URL);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SendWebRequest();
        while (!www.isDone)
        {
            // Not really suported with github, here as a representation.
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
            SaveManager.SetString("ConfigData", www.downloadHandler.text);
        }
        GameManager.Instance.LoadData(SaveManager.GetString("ConfigData"));


    }
}
