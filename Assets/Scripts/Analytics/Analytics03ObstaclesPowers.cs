using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class Analytics03ObstaclesPowers : MonoBehaviour
{
    private string URL;

    private void Awake()
    {
        URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLScSof7c_5OT_nEkbgvv473mRJLW2SIy1-nHwNFU0vnJJHGKGw/formResponse";
    }

    public void Send(long sessionId, string checkpointName, string levelName, string ObstacleOrPower, long totalSoFar)
    {
        // Debug.Log("SEND is called");
        if (PlayerMovement.analytics02Enabled==false){
            return;
        }

        //Debug.Log("SEND CO-routine is called");
        StartCoroutine(Post(sessionId.ToString(), checkpointName, levelName, ObstacleOrPower, totalSoFar.ToString()));
    }

    private IEnumerator Post(string sessionID, string checkpointName, string levelName, string ObstacleOrPower, string totalHitsSoFar)
    {
        // Create the form and enter responses
        //Debug.Log("FORMS is being is called");
        WWWForm form = new WWWForm();
        // Somehow the order is twisted here, fill carefully
        form.AddField("entry.2140576380", sessionID);
        form.AddField("entry.1195628609", checkpointName);
        form.AddField("entry.2036311013", levelName);
        form.AddField("entry.1770785179", ObstacleOrPower);
        form.AddField("entry.1704716457", totalHitsSoFar);      
        
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
            //Debug.Log("Forms3 upload complete!");
        }

        www.Dispose();
        // form.Dispose();
    }
}
