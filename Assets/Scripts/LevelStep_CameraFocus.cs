// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_CameraFocus : LevelStep
{
	public GameObject chaser;

	public GameObject target1;

	public GameObject target2;

	public Vector3 targetPosition;

	public Vector3 offset;

	public LevelStep_CameraFocus(GameObject target1, GameObject target2, Vector3 position, Vector3 offset)
	{
		this.target1 = target1;
		this.target2 = target2;
		this.targetPosition = position;
		this.offset = offset;
	}

	public override void Start()
	{
		base.Start();
		this.chaser = SceneManager.instance.chaserObject;
		if (this.target1 != null)
		{
			if (this.target2 != null)
			{
				this.targetPosition = (this.target2.transform.position - this.target1.transform.position) / 2f + this.target1.transform.position;
			}
			else
			{
				this.targetPosition = this.target1.transform.position;
			}
		}
		this.chaser.transform.position = this.targetPosition + this.offset;
	}

	public override bool Update()
	{
		base.Update();
		return true;
	}
}
