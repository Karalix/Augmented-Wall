using UnityEngine;
using System.Timers;
using System.IO;
using SimpleJSON;
using System.Collections;

public class WeatherService : MonoBehaviour{


    public float changeRate;
    private float changeCooldown;

    static WeatherService _instance;
    public static WeatherService Instance
    {
        get
        {
            return _instance;
        }
    }

    private const string weatherApiKey = "1c115fe97a05be2fa4fc4e25a3da74b7" ;

    public string City { get; private set; }
    public string MainWeather { get; private set; }
    public float Temperature { get; private set; }
    public string MainWeatherImage { get; private set; }

    private void OnTimedEvent()
    {
        WWW req = new WWW("http://api.openweathermap.org/data/2.5/weather?q=Orsay,fr&units=metric&appid="+weatherApiKey);

        StartCoroutine(WaitForRequest(req));

    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            var response = JSON.Parse(www.text);
            City = response["name"].Value;
            MainWeather = response["weather"][0]["main"].Value;
            Temperature = response["main"]["temp"].AsFloat;
            MainWeatherImage = "http://openweathermap.org/img/w/" + response["weather"][0]["icon"].Value + ".png";
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    // Use this for initialization
    void Start () {

        _instance = this;

        changeCooldown = 0f;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (changeCooldown > 0)
        {
            changeCooldown -= Time.deltaTime;
        }
        else
        {
            changeCooldown = changeRate;
            OnTimedEvent();
        }
    }
}
