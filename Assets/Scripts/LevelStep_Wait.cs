// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_Wait : LevelStep
{
	private float duration;

	public LevelStep_Wait(float duration)
	{
		this.duration = duration;
	}

	public override void Start()
	{
		base.Start();
	}

	public override bool Update()
	{
		base.Update();
		return this.startTime + this.duration < Time.unscaledTime;
	}
}
