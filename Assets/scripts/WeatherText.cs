﻿using UnityEngine;
using System.Collections;

public class WeatherText : MonoBehaviour {
    public float changeRate = 1f;
    private float changeCooldown;

    // Use this for initialization
    void Start()
    {
        changeCooldown = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (changeCooldown > 0)
        {
            changeCooldown -= Time.deltaTime;
        }
        else {
            changeCooldown = changeRate;

            var textMesh = GetComponent<TextMesh>();
            if (textMesh != null)
            {
                textMesh.text = WeatherService.Instance.MainWeather;
            }
        }
    }
}
