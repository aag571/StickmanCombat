// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class FPSCartoon : MonoBehaviour
{
	private readonly GUIStyle guiStyleHeader = new GUIStyle();

	private float timeleft;

	private float fps;

	private int frames;

	private void Awake()
	{
		this.guiStyleHeader.fontSize = 14;
		this.guiStyleHeader.normal.textColor = new Color(1f, 1f, 1f);
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0f, 0f, 30f, 30f), "帧率：" + (int)this.fps, this.guiStyleHeader);
	}

	private void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			this.fps = (float)this.frames;
			this.timeleft = 1f;
			this.frames = 0;
		}
	}
}
