// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_Speech : LevelStep
{
	public string text;

	public GameObject targetObject;

	private float duration;

	private bool textEnded;

	private bool timed;

	public LevelStep_Speech(string text, GameObject targetObject, float duration = 2.5f, bool timed = false)
	{
		this.text = text;
		this.targetObject = targetObject;
		this.duration = duration;
		this.timed = timed;
	}

	public override void Start()
	{
		base.Start();
		this.textEnded = false;
		DialogManager.instance.Open(this.text, this.timed, this.duration);
		DialogManager.instance.relativeText.target = this.targetObject;
		DialogManager.instance.relativeText.Start();
	}

	public override bool Update()
	{
		base.Update();
		if (this.timed)
		{
			return true;
		}
		if (DialogManager.instance.charCount >= DialogManager.instance.targetString.Count - 1)
		{
			if (!this.textEnded)
			{
				this.textEnded = true;
				this.startTime = Time.unscaledTime;
			}
			if (Input.GetMouseButtonDown(0))
			{
				DialogManager.instance.Close();
				return true;
			}
			if (this.startTime + this.duration < Time.unscaledTime)
			{
				DialogManager.instance.Close();
				return true;
			}
		}
		return false;
	}
}
