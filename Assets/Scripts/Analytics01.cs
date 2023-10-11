using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Analytics01 : MonoBehaviour
{
    private string URL;
    private long sessionId;
    private int testInt;
    
    private string checkpointName; // Look into this? the accessibility
    private string levelName;
    private float timeTaken;

    private void Awake()
    {
        //sessionId = DateTime.Now.Ticks;
        URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLScRQv83I1oLYwYwnpucIUAv5anjT6hIB-HTqILrXkoFefMnrw/formResponse";
    }

    public void Send(string checkpointName, float timeTaken,string levelName,long sessionId)
    {

        if (PlayerMovement.analytics01Enabled==false){
            return;
        }

        Debug.Log(checkpointName);
        Debug.Log(timeTaken);
        sessionId=sessionId;

        if (timeTaken==0){
            return;
        }
        checkpointName = checkpointName;
        testInt = UnityEngine.Random.Range(0, 101);
        
        
        levelName=levelName;

        StartCoroutine(Post(sessionId.ToString(), checkpointName, timeTaken.ToString(), checkpointName, levelName));
    }

    private IEnumerator Post(string sessionID, string testInt, string checkpointName, string timeTaken, string levelName)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.1383666950", sessionID);
        form.AddField("entry.360401964", testInt);
        
        
        form.AddField("entry.1650855500", checkpointName);
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
            Debug.Log("Form upload complete!");
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
