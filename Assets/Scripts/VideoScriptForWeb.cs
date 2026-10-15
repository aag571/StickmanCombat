// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class VideoScriptForWeb : MonoBehaviour
{
	public AmbientLight al;

	public NpcManager npcManager;

	private void Start()
	{
		this.al.speed = 0.1f;
		WeatherManager.instance.ChangeWeather();
		base.InvokeRepeating("ChangeWeather", 6f, 6f);
	}

	private void Update()
	{
		this.npcManager.gameStartTime = (double)(Time.time - 1000f);
	}

	private void ChangeWeather()
	{
		WeatherManager.instance.ChangeWeather();
		WeatherManager.instance.ChangeWeather();
	}
}
