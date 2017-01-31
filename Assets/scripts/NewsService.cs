using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class NewsService : MonoBehaviour {


    public float changeRate;
    private float changeCooldown;

    public ArrayList Stories { get; private set; }

    private ArrayList TmpStories;

    static NewsService _instance;
    public static NewsService Instance
    {
        get
        {
            return _instance;
        }
    }

    private void OnTimedEvent()
    {
        WWW req = new WWW("https://hacker-news.firebaseio.com/v0/topstories.json");

        StartCoroutine(WaitForTopStoryRequest(req));

    }

    IEnumerator WaitForTopStoryRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            ArrayList topTenStories = new ArrayList();
            var response = JSON.Parse(www.text);
            for(int i = 0; i < 10; i++)
            {
                topTenStories.Add(response[i].Value);
            }
            Debug.Log(topTenStories[0]);

            TmpStories.Clear();

            foreach(string story in topTenStories)
            {
                WWW req = new WWW("https://hacker-news.firebaseio.com/v0/item/"+story+".json");
                StartCoroutine(WaitForStoryRequest(req));
            }
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    IEnumerator WaitForStoryRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            var response = JSON.Parse(www.text);
            var title = response["title"].Value;
            var score = response["score"].AsInt;
            var time = response["time"].AsInt;
            TmpStories.Add(new Story(title,score, time));
            if(TmpStories.Count == 10)
            {
                Stories.Clear();
                Stories.AddRange(TmpStories);
            }
            //Debug.Log(title);
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    // Use this for initialization
    void Start()
    {
        _instance = this;
        Stories = new ArrayList();
        TmpStories = new ArrayList();
        changeCooldown = 0f;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (changeCooldown > 0)
        {
            changeCooldown -= Time.deltaTime;
        }else
        {
            changeCooldown = changeRate;
            OnTimedEvent();
        }
    }

    
}

public class Story
{
    public string Title;
    public int score;
    public DateTime time;

    public Story(string title, int score, int time)
    {
        Title = title;
        this.score = score;
        this.time = UnixTimeStampToDateTime(time);
    }

    public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
}