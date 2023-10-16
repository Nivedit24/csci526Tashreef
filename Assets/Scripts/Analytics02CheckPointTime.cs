using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class Analytics02CheckPointTime : MonoBehaviour
{
    private string URL;
    private long sessionIdGlobal;
    private int testInt;
    
    private string checkpointNameGlobal; 
    private string levelNameGlobal;
    private float timeTakenCheckPoint;
    private float timeTakenTotal;

    private void Awake()
    {
        sessionIdGlobal = DateTime.Now.Ticks;
        URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLSf52CGJmwp3H7iw9Cef0rCYPyyP5X946Uk5F0FPwhptj5OTcQ/formResponse";
    }

    public void Send(long sessionId, string checkpointName, string levelName, double timeTakenCheckPoint, double timeTakenTotal, long totalAttempts)
    {
        // Debug.Log("SEND is called");
        if (PlayerMovement.analytics02Enabled==false){
            return;
        }

        // Debug.Log(checkpointName);
        // Debug.Log(timeTaken);
        sessionIdGlobal = sessionId;

        // if (timeTaken==0){
        //     return;
        // }

        checkpointNameGlobal = checkpointName;
        testInt = UnityEngine.Random.Range(0, 101);
        
        
        levelNameGlobal = levelName;
        
        //Debug.Log("SEND CO-routine is called");
        StartCoroutine(Post(sessionId.ToString(), checkpointName, levelName, timeTakenCheckPoint.ToString(), timeTakenTotal.ToString(), totalAttempts.ToString()));
    }

    private IEnumerator Post(string sessionID, string checkpointName, string levelName, string timeTakenCheckPoint, string timeTakenTotal, string totalAttempts)
    {
        // Create the form and enter responses
        //Debug.Log("FORMS is being is called");
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
            Debug.Log("Forms2 upload complete!");
        }

        www.Dispose();
        // form.Dispose();
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
