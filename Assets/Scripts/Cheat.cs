// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class Cheat : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		SceneManager.instance.isBeltExamReady = true;
		StoryManager.instance.level = 15;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//AudioManager.instance.ThunderSound();
			//WeatherManager.instance.lightningSource.transform.localPosition = WeatherManager.instance.sourcePos;
			//WeatherManager.instance.lightningDest.transform.localPosition = WeatherManager.instance.destPos;
			//WeatherManager.instance.lightning.Trigger();
			//base.Invoke("TriggerLightning", UnityEngine.Random.Range(0.2f, 0.6f));
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			WeatherManager.instance.ChangeWeather();
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			Time.timeScale = 0f;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			Time.timeScale = 1f;
		}
	}
}
