// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VIDEOSCENE3 : MonoBehaviour
{
	private bool started;

	public Animator fadeAnim;

	public global::SceneManager ssm;

	public NpcManager npcManager;

	public AmbientLight ambientLight;

	private int count;

	private void Start()
	{
		this.npcManager.gameObject.SetActive(false);
		base.InvokeRepeating("ChangeItems", 1f, 0.5f);
		WeatherManager.instance.ChangeWeather();
		base.InvokeRepeating("ChangeWeather", 3f, 3f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		if (this.started)
		{
			Camera.main.transform.position += new Vector3(0f, 0f, Time.deltaTime / 2f);
		}
	}

	public void ChangeWeather()
	{
		WeatherManager.instance.ChangeWeather();
		WeatherManager.instance.ChangeWeather();
	}

	public void ChangeItems()
	{
		if (!this.started)
		{
			this.ambientLight.t = 0.5f;
			this.fadeAnim.Play("faderOpen");
		}
		this.started = true;
		this.count++;
		int num = 0;
		foreach (KeyValuePair<string, Weapon> current in ItemManager.instance.weaponsById)
		{
			if (num == this.count)
			{
				ItemManager.instance.currentWeapon = current.Value;
				ItemManager.instance.EquipCurrentWeapon(false);
			}
			num++;
		}
		num = 0;
		foreach (KeyValuePair<string, Hat> current2 in ItemManager.instance.hatsById)
		{
			if (num == this.count)
			{
				ItemManager.instance.currentHat = current2.Value;
				ItemManager.instance.EquipCurrentHat(false);
			}
			num++;
		}
		this.count++;
	}
}
