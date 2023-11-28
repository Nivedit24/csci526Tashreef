using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class Analytics02CheckPointTime : MonoBehaviour
{
    private string URL;
    
    private void Awake()
    {
        URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLSf52CGJmwp3H7iw9Cef0rCYPyyP5X946Uk5F0FPwhptj5OTcQ/formResponse";
    }

    public void Send(long sessionId, string checkpointName, string levelName, double timeTakenCheckPoint, double timeTakenTotal, long totalAttempts)
    {
        // Debug.Log("SEND is called");
        if (PlayerMovement.analytics02Enabled==false)
        {
            return;
        }
   
        //Debug.Log("SEND CO-routine is called");
        StartCoroutine(Post(sessionId.ToString(), checkpointName, levelName, timeTakenCheckPoint.ToString(), timeTakenTotal.ToString(), totalAttempts.ToString()));
    }

    private IEnumerator Post(string sessionID, string checkpointName, string levelName, string timeTakenCheckPoint, string timeTakenTotal, string totalAttempts)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.145303953", sessionID);
        form.AddField("entry.215474747", checkpointName);
        form.AddField("entry.909676238", levelName);
        form.AddField("entry.1102669765", timeTakenCheckPoint);
        form.AddField("entry.1195121878", timeTakenTotal);
        form.AddField("entry.1557255540", totalAttempts);
        
        
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        
        www.disposeUploadHandlerOnDispose = true;
        www.disposeDownloadHandlerOnDispose = true;
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(URL);
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Forms2 upload complete!");
        }

        www.Dispose();
        // form.Dispose();
    }
}
