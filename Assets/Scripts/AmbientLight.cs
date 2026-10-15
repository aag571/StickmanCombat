// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class AmbientLight : MonoBehaviour
{
	public float speed = 0.1f;

	public Gradient ambientLight;

	public Gradient fog;

	public Gradient sunFlarePower;

	public Gradient moonFlarePower;

	public Gradient lightPower;

	public Transform skyPlane;

	public AnimationCurve rotationCurve;

	public Color ambientLightColor;

	public Color fogColor;

	public float t;

	public Material sky;

	public Material sunFlare;

	public Material moonFlare;

	public Light gameLight;

	public SpriteRenderer[] playerGlows;

	public float starAlpha;

	public float glowFactor = 0.4f;

	public Color snowFog;

	public Color rainFog;

	private void Start()
	{
		this.t = PlayerPrefs.GetFloat("AmbientTime", 0.7f);
		this.starAlpha = -1f;
		this.Update();
	}

	private void Update()
	{
		this.t += Time.deltaTime * this.speed;
		RenderSettings.ambientLight = this.ambientLight.Evaluate(this.t);
		this.ambientLightColor = RenderSettings.ambientLight;
		if (WeatherManager.instance.activeWeather == 2)
		{
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, this.rainFog, Time.deltaTime * 10f);
		}
		else if (WeatherManager.instance.activeWeather == 3)
		{
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, this.snowFog, Time.deltaTime * 10f);
		}
		else
		{
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, this.fog.Evaluate(this.t), Time.deltaTime * 10f);
		}
		this.fogColor = RenderSettings.fogColor;
		this.skyPlane.localEulerAngles = -Vector3.forward * this.rotationCurve.Evaluate(this.t);
		if (WeatherManager.instance.activeWeather > 1)
		{
			this.sunFlare.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
			this.moonFlare.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
		}
		else
		{
			this.sunFlare.SetColor("_Color", new Color(1f, 1f, 1f, this.sunFlarePower.Evaluate(this.t).a));
			this.moonFlare.SetColor("_Color", new Color(1f, 1f, 1f, this.moonFlarePower.Evaluate(this.t).a));
		}
		if (this.gameLight != null)
		{
			this.gameLight.color = this.lightPower.Evaluate(this.t);
		}
		if (this.t > 1f)
		{
			this.t = 0f;
		}
		float num;
		if (this.t < 0.2f || this.t > 0.8f)
		{
			num = 0f;
		}
		else if ((this.t > 0.3f && this.t < 0.5f) || (this.t < 0.7f && this.t > 0.5f))
		{
			num = 1f;
		}
		else if (this.t < 0.5f)
		{
			num = (this.t - 0.2f) / 0.1f;
		}
		else
		{
			num = 1f - (this.t - 0.7f) / 0.1f;
		}
		if (RenderSettings.fogDensity > 0.05f)
		{
			num = Mathf.Max(num, 1f - (0.1f - RenderSettings.fogDensity) * 20f);
		}
		if (this.starAlpha != num)
		{
			this.starAlpha = num;
			
			this.UpdatePlayerGlow();
			
		}
	}

	public void UpdatePlayerGlow()
	{
		SpriteRenderer[] array = this.playerGlows;
		for (int i = 0; i < array.Length; i++)
		{
			SpriteRenderer spriteRenderer = array[i];
			spriteRenderer.color = new Color(1f, 1f, 1f, this.glowFactor);
		}
	}
}
