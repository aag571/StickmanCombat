// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class Video2ItemEquip : MonoBehaviour
{
	private void Start()
	{
		base.Invoke("StartNow", 1f);
	}

	private void StartNow()
	{
		ItemManager.instance.currentHat = ItemManager.instance.hatsById["sparrow"];
		ItemManager.instance.currentWeapon = ItemManager.instance.weaponsById["sword1h3"];
		ItemManager.instance.EquipCurrentWeapon(false);
		ItemManager.instance.EquipCurrentHat(false);
	}

	private void Update()
	{
	}
}
