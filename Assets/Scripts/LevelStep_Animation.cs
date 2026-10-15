// 反编译自程序集： Assembly-CSharp.dll
using Spine.Unity;
using System;
using UnityEngine;

public class LevelStep_Animation : LevelStep
{
	public SkeletonAnimation spine;

	public string animationName;

	private bool loop;

	private bool flip;

	private float duration;

	public LevelStep_Animation(SkeletonAnimation spine, string animationName, bool loop, float duration = 0f, bool flip = false)
	{
		this.spine = spine;
		this.animationName = animationName;
		this.loop = loop;
		this.flip = flip;
		this.duration = duration;
	}

	public override void Start()
	{
		base.Start();
		if (this.animationName != string.Empty)
		{
			this.spine.state.SetAnimation(0, this.animationName, this.loop);
		}
		this.spine.skeleton.flipX = this.flip;
	}

	public override bool Update()
	{
		base.Update();
		return this.startTime + this.duration <= Time.unscaledTime;
	}
}
