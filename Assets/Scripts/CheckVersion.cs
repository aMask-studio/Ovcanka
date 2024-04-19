using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
struct GetdataOutput //структура для чтения json файла конфигурации
{
    public string version; //номер версии
    public string link; //ссылка на apk файл
}
public class CheckVersion : MonoBehaviour
{
    public string AppName; //Название APK файла (без расширения .apk), например LiteraryTriangle
    public string LinkToJson; //Ссылка до файла конфигурации json (вместе с расширением .json),
                              //например http://website/folder/version.json

    public TMP_Text txtLoading; //текст со статусом загрузки
    private void Start()
    {
        StartCoroutine(GetRequest());
    }
    /// <summary>
    /// Проверка новой версии с сервера
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetRequest()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://koyltoh4.beget.tech/Ovcanka/version.json");
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError)
        {
            UnityEngine.Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            GetdataOutput all_data = (GetdataOutput)JsonUtility.FromJson(uwr.downloadHandler.text, typeof(GetdataOutput));
            if (all_data.version != Application.version)
            {
                if (all_data.link != null)
                {
                    txtLoading.gameObject.SetActive(true);
                    txtLoading.text = $"Скачивается обновление (v{all_data.version})";
                    StartCoroutine(GetFile(all_data.link));
                }
            }
            UnityEngine.Debug.Log(all_data.version);
            UnityEngine.Debug.Log(all_data.link);
        }
    }
    /// <summary>
    /// Установка новой версии с сервера
    /// </summary>
    /// <param name="link"></param>
    /// <returns></returns>
    IEnumerator GetFile(string link)
    {
        var wwwRequest = new UnityWebRequest(link);
        wwwRequest.method = UnityWebRequest.kHttpVerbGET;

        var dh = new DownloadHandlerFile(Application.persistentDataPath + $"/{AppName}.apk");
        dh.removeFileOnAbort = true;
        wwwRequest.downloadHandler = dh;
        if (wwwRequest.isDone != true)
        {
            UnityEngine.Debug.Log(wwwRequest.downloadProgress);
            UnityEngine.Debug.Log(wwwRequest.isDone);
        }
        yield return wwwRequest.SendWebRequest();
        if (wwwRequest.isNetworkError || wwwRequest.isHttpError)
        {
            UnityEngine.Debug.Log(wwwRequest.error);
            txtLoading.text = $"APK файла по ссылке {link} не найден.";
        }
        else
        {
            txtLoading.text = "Скачано. APK файл находится в папке Download";
            InstallApp(Application.persistentDataPath + $"/{AppName}.apk");

            //Резервное копирование APK в папку Download
            File.Copy(Application.persistentDataPath + $"/{AppName}.apk", "/storage/emulated/0/Download" + $"/{AppName}.apk", true);
            UnityEngine.Debug.Log("Downloaded");
        }

        yield return wwwRequest;
    }
    /// <summary>
    /// Установка приложения из APK файла на устройство
    /// </summary>
    /// <param name="apkPath"></param>
    public void InstallApp(string apkPath) 
    {
        txtLoading.text = "APK устанавливается";

        //Get Activity then Context
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        //Get the package Name
        string packageName = unityContext.Call<string>("getPackageName");
        string authority = packageName + ".fileprovider";

        AndroidJavaClass intentObj = new AndroidJavaClass("android.content.Intent");
        string ACTION_VIEW = intentObj.GetStatic<string>("ACTION_VIEW");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);


        int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
        int FLAG_GRANT_READ_URI_PERMISSION = intentObj.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");

        //File fileObj = new File(String pathname);
        AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", apkPath);
        //FileProvider object that will be used to call it static function
        AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
        //getUriForFile(Context context, String authority, File file)
        AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority, fileObj);

        intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
        intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
        intent.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
        currentActivity.Call("startActivity", intent);
        txtLoading.text = "APK установлен";
    }
}
