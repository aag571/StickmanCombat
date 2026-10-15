// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
	public static WeatherManager instance;


	public int activeWeather;

	public ParticleSystem rainSystem;

	public ParticleSystem dropSystem;

	public ParticleSystem emberSystem;

	public ParticleSystem snowSystem;

	public double nextWeatherChangeTime;

	public double nextLightningTime;

	public GameObject lightningSource;

	public GameObject lightningDest;

	public List<int> weatherList;

	public MenuToggle weatherToggler;

	public Vector3 sourcePos;

	public Vector3 destPos;

	private void Awake()
	{
		WeatherManager.instance = this;
	}

	private void Start()
	{
		this.nextWeatherChangeTime = (double)(Time.time + (float)UnityEngine.Random.Range(80, 220));
		this.sourcePos = this.lightningSource.transform.localPosition;
		this.destPos = this.lightningDest.transform.localPosition;
		this.weatherList = new List<int>();
		this.FillWeatherList();
		if (this.weatherToggler.isOn)
		{
			this.lightningSource.transform.localPosition = this.sourcePos;
			this.lightningDest.transform.localPosition = this.destPos;
			this.lightningSource.transform.localPosition = this.sourcePos;
			this.lightningDest.transform.localPosition = this.sourcePos;
			base.Invoke("TriggerLightning", 0.1f);
		}
	}

	private void Update()
	{
		if (!this.weatherToggler.isOn)
		{
			if (this.emberSystem.isPlaying)
			{
				this.emberSystem.Stop();
			}
			if (this.rainSystem.isPlaying)
			{
				this.rainSystem.Stop();
			}
			if (this.dropSystem.isPlaying)
			{
				this.dropSystem.Stop();
			}
			if (this.snowSystem.isPlaying)
			{
				this.snowSystem.Stop();
			}
		}
		base.transform.position = new Vector3(Camera.main.transform.position.x, base.transform.position.y, base.transform.position.z);
		if ((double)Time.time > this.nextWeatherChangeTime)
		{
			this.ChangeWeather();
		}
		if (this.activeWeather > 1)
		{
			RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, 0.1f, Time.deltaTime / 20f);
		}
		else
		{
			RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, 0.05f, Time.deltaTime / 20f);
		}
		if (this.activeWeather == 2 && this.weatherToggler.isOn && (double)Time.time > this.nextLightningTime)
		{
			this.nextLightningTime = (double)((float)UnityEngine.Random.Range(5, 15) + Time.time);
			this.lightningSource.transform.localPosition = this.sourcePos;
			this.lightningDest.transform.localPosition = this.destPos;
			//AudioManager.instance.ThunderSound();
			base.Invoke("TriggerLightning", UnityEngine.Random.Range(0.2f, 0.6f));
		}
	}

	public void ChangeWeather()
	{
		if (this.activeWeather == 0)
		{
			if (this.weatherList.Count <= 0)
			{
				this.FillWeatherList();
			}
			this.activeWeather = this.weatherList[0];
			this.weatherList.RemoveAt(0);
			this.nextWeatherChangeTime = (double)(Time.time + (float)UnityEngine.Random.Range(40, 80));
			if (this.weatherToggler.isOn)
			{
				int num = this.activeWeather;
				if (num != 1)
				{
					if (num != 2)
					{
						if (num == 3)
						{
							this.snowSystem.Play();
						}
					}
					else
					{
						this.nextLightningTime = (double)((float)UnityEngine.Random.Range(5, 15) + Time.time);
						this.rainSystem.Play();
						this.dropSystem.Play();
					}
				}
				else
				{
					this.emberSystem.Play();
				}
			}
		}
		else
		{
			this.nextWeatherChangeTime = (double)(Time.time + (float)UnityEngine.Random.Range(80, 220));
			int num2 = this.activeWeather;
			if (num2 != 1)
			{
				if (num2 != 2)
				{
					if (num2 == 3)
					{
						this.snowSystem.Stop();
					}
				}
				else
				{
					this.rainSystem.Stop();
					this.dropSystem.Stop();
				}
			}
			else
			{
				this.emberSystem.Stop();
			}
			this.activeWeather = 0;
		}
	}

	public void FillWeatherList()
	{
		List<int> list = new List<int>();
		list.Add(1);
		list.Add(2);
		list.Add(3);
		this.weatherList.Clear();
		while (list.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			this.weatherList.Add(list[index]);
			list.RemoveAt(index);
		}
	}

	public void TriggerLightning()
	{
		if (this.weatherToggler.isOn)
		{
			
		}
	}
}
