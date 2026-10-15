// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class VIDEOSCENE4 : MonoBehaviour
{
	private void Start()
	{
		foreach (KeyValuePair<string, Weapon> current in ItemManager.instance.weaponsById)
		{
			current.Value.Own();
		}
		foreach (KeyValuePair<string, Hat> current2 in ItemManager.instance.hatsById)
		{
			current2.Value.Own();
		}
		StoryManager.instance.level = 16;
	}

	private void Update()
	{
		SceneManager.instance.isBeltExamReady = true;
		if (Input.GetKeyDown(KeyCode.F1))
		{
			StoryManager.instance.level = 0;
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			StoryManager.instance.level = 2;
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			StoryManager.instance.level = 8;
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			StoryManager.instance.level = 10;
		}
		if (Input.GetKeyDown(KeyCode.F5))
		{
			StoryManager.instance.level = 14;
		}
		if (Input.GetKeyDown(KeyCode.F6))
		{
			StoryManager.instance.level = 9;
		}
	}
}
