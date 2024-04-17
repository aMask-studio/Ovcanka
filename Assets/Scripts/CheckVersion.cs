using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
struct GetdataOutput
{
    public string version;
    public string link;
}
public class CheckVersion : MonoBehaviour
{
    public TMP_Text txtLoading;
    private void Start()
    {
        StartCoroutine(GetRequest());
    }
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
            UnityEngine.Debug.Log(uwr.downloadHandler.text);
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
    IEnumerator GetFile(string link)
    {
        var wwwRequest = new UnityWebRequest(link);
        wwwRequest.method = UnityWebRequest.kHttpVerbGET;
        UnityEngine.Debug.Log(Application.persistentDataPath);

        var dh = new DownloadHandlerFile(Application.persistentDataPath + "/LiteraryTriangle.apk");
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
        }
        else
        {
            txtLoading.text = "Скачано. APK файл находится в папке Download";
            installApp(Application.persistentDataPath + "/LiteraryTriangle.apk");
            File.Copy(Application.persistentDataPath + "/LiteraryTriangle.apk", "/storage/emulated/0/Download" + "/LiteraryTriangle.apk", true);
            //File.Move(Application.persistentDataPath + "/LiteraryTriangle.apk","/storage/emulated/0/Download" + "/LiteraryTriangle.apk");
            UnityEngine.Debug.Log("success");
        }

        yield return wwwRequest;
    }
    public void installApp(string apkPath)
    {
        //bool success = true;
        //GameObject.Find("TextDebug").GetComponent<Text>().text = "Installing App";
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

        /*try
        {
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

            //GameObject.Find("TextDebug").GetComponent<Text>().text = "Success";
        }
        catch (System.Exception e)
        {
            txtLoading.text = "Ошибка" + e.Message;
            //GameObject.Find("TextDebug").GetComponent<Text>().text = "Error: " + e.Message;
            success = false;
        }*/

        //return success;
    }
}
