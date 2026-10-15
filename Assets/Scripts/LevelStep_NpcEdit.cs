// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_NpcEdit : LevelStep
{
	private Npc npc;

	private int level;

	private float speed;

	public LevelStep_NpcEdit(Npc npc, int level, float speed = -1f)
	{
		this.npc = npc;
		this.level = level;
		this.speed = speed;
	}

	public override void Start()
	{
		base.Start();
		int num = 10 + this.level * 4;
		if (this.level > 15)
		{
			//if (LeaderboardsManager.instance.shadowWarriorData[0] != SystemInfo.deviceUnique类型标识)
			//{
			//	num = int.Parse(LeaderboardsManager.instance.shadowWarriorData[4]) + 1;
			//}
			//else
			//{
			//	num = 500;
			//}
		}
		if (this.speed == -1f)
		{
			this.speed = 3f + (float)this.level * 0.166666672f;
			if (this.speed > 5.5f)
			{
				this.speed = 5.5f;
			}
			UnityEngine.Random.InitState(this.level);
			for (int i = 0; i < num; i++)
			{
				this.npc.hitSides.Add(UnityEngine.Random.Range(0, 2) * 2 - 1);
			}
			this.npc.Activate();
			this.npc.speed = this.speed;
		}
		else
		{
			this.npc.speed = this.speed;
		}
	}

	public override bool Update()
	{
		base.Update();
		return true;
	}
}
