// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemChangeForVideo : MonoBehaviour
{
	private bool started;

	public Animator fadeAnim;

	public SceneManager ssm;

	private int count;

	private void Start()
	{
		SceneManager.instance = this.ssm;
		base.InvokeRepeating("ChangeItems", 1f, 0.5f);
	}

	private void Update()
	{
		if (this.started)
		{
			Camera.main.transform.position += new Vector3(0f, 0f, Time.deltaTime / 2f);
		}
	}

	public void ChangeItems()
	{
		if (!this.started)
		{
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
