// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_Tip : LevelStep
{
	public string text;

	private bool open;

	private float duration;

	private bool timed;

	public LevelStep_Tip(string text, bool open, float duration = 0f, bool timed = false)
	{
		this.text = text;
		this.open = open;
		this.timed = timed;
		this.duration = duration;
	}

	public override void Start()
	{
		base.Start();
		if (this.open)
		{
			TipManager.instance.Open(this.text, this.timed, this.duration);
		}
	}

	public override bool Update()
	{
		base.Update();
		if (this.timed)
		{
			return true;
		}
		if (this.duration + this.startTime < Time.unscaledTime)
		{
			if (!this.open)
			{
				TipManager.instance.Close(false);
			}
			return true;
		}
		return false;
	}
}
