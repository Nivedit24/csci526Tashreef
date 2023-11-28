using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class Analytics01DeadTime : MonoBehaviour
{
    private string URL;

    private void Awake()
    {
        URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLScRQv83I1oLYwYwnpucIUAv5anjT6hIB-HTqILrXkoFefMnrw/formResponse";
    }

    public void Send(long sessionId, string xCord, string yCord, string levelName)
    {
        //Debug.Log("SEND is called");
        if (PlayerMovement.analytics01Enabled==false){
            return;
        }

        //Debug.Log("SEND CO-routine is called");
        StartCoroutine(Post(sessionId.ToString(), xCord, yCord, "death co-ordinates", levelName));
    }

    private IEnumerator Post(string sessionID, string xCord, string yCord, string timeTaken, string levelName)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.1383666950", sessionID);
        form.AddField("entry.360401964", xCord);
        
        
        form.AddField("entry.1650855500", yCord);
        form.AddField("entry.953686723", timeTaken);
        form.AddField("entry.1294741655", levelName);
        
        
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
            //Debug.Log("Forms upload complete!");
        }

        www.Dispose();
        // form.Dispose();
    }
}
