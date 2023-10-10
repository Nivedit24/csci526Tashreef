using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Analytics01 : MonoBehaviour
{
    private string URL;
    private long _sessionId;
    private int _testInt;
    
    private string _checkpoint_name;
    private string _level_name;
    private float _time_taken;

    private void Awake()
    {
        //_sessionId = DateTime.Now.Ticks;
        URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLScRQv83I1oLYwYwnpucIUAv5anjT6hIB-HTqILrXkoFefMnrw/formResponse";
    }

    public void Send(string checkpoint_name, float time_taken,string level_name,long sessionId)
    {

        if (PlayerMovement.analytics_01_enabled==false){
            return;
        }

        Debug.Log(checkpoint_name);
        Debug.Log(time_taken);
        _sessionId=sessionId;

        if (time_taken==0){
            return;
        }
        _checkpoint_name = checkpoint_name;
        _testInt = UnityEngine.Random.Range(0, 101);
        
        
        _level_name=level_name;

        StartCoroutine(Post(_sessionId.ToString(), checkpoint_name, time_taken.ToString(), _checkpoint_name, _level_name));
    }

    private IEnumerator Post(string sessionID, string testInt, string _checkpoint_name, string _time_taken, string _level_name)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.1383666950", sessionID);
        form.AddField("entry.360401964", testInt);
        
        
        form.AddField("entry.1650855500", _checkpoint_name);
        form.AddField("entry.953686723", _time_taken);
        form.AddField("entry.1294741655", _level_name);
        
        
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
