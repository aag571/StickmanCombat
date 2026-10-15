// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScreenshotForIaps : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			ScreenCapture.CaptureScreenshot("ScreenShots/Hats/" + ItemManager.instance.currentHat.id + ".png");
		}
	}
}
