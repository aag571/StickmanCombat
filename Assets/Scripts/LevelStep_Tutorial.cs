// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_Tutorial : LevelStep
{
	public bool spawn;

	public bool check;

	private float speed;

	public int type;

	public int side;

	public LevelStep_Tutorial(bool spawn, bool check, int type, float speed, int side)
	{
		this.spawn = spawn;
		this.check = check;
		this.type = type;
		this.speed = speed;
		this.side = side;
	}

	public override void Start()
	{
		base.Start();
		if (this.spawn)
		{
			NpcManager.instance.SpawnEnemy(this.side, StoryManager.instance.GetWorldPositionOnPlane(new Vector3((float)Screen.width, 0f, 0f)).x + 2f - CharacterManager.instance.transform.position.x, this.speed, this.type);
		}
	}

	public override bool Update()
	{
		base.Update();
		if (!this.check)
		{
			return true;
		}
		Npc value;
		if (this.side == 1)
		{
			if (NpcManager.instance.rightNpcs.Count == 0)
			{
				value = NpcManager.instance.leftNpcs.First.Value;
			}
			else
			{
				value = NpcManager.instance.rightNpcs.First.Value;
			}
		}
		else
		{
			value = NpcManager.instance.leftNpcs.First.Value;
		}
		if (this.type == 0)
		{
			if (Time.timeScale > 0f && value.markedForKill && CharacterManager.instance.currentState == CharacterManager.State.Idle)
			{
				Time.timeScale = 0f;
				TipManager.instance.Close(true);
				if (this.side == 1)
				{
					TipManager.instance.Open("#点击 #右 攻击！", false, 0f);
				}
				else
				{
					TipManager.instance.Open("#点击 #左 攻击！", false, 0f);
				}
			}
			if (Time.timeScale == 0f && TipManager.instance.charCount >= TipManager.instance.targetString.Count - 1 && Input.GetMouseButtonDown(0) && Input.mousePosition.x > (float)(Screen.width / 2) == (this.side == 1))
			{
				Time.timeScale = 1f;
				CharacterManager.instance.inputQueue.Enqueue(this.side);
				TipManager.instance.Close(true);
				if (this.side == 1)
				{
					TipManager.instance.Open("干得好， #等待 另一个人", false, 0f);
				}
				else
				{
					TipManager.instance.Open("完美！小心剩下的人。", true, 3f);
				}
				return true;
			}
		}
		else if (this.type == 2 || (this.type == 3 && this.side == 1))
		{
			if (value.transform.position.x < StoryManager.instance.GetWorldPositionOnPlane(new Vector3((float)Screen.width, 0f, 0f)).x - 1f && this.speed == 3f)
			{
				StoryManager.instance.levelSteps.Enqueue(new LevelStep_NpcEdit(value, 0, -1f));
				StoryManager.instance.levelSteps.Enqueue(new LevelStep_Animation(value.spine, "FistIdle", true, 0f, true));
				if (this.type == 2)
				{
					StoryManager.instance.levelSteps.Enqueue(new LevelStep_Speech("红色匕首，要小心。", CharacterManager.instance.head, 2.5f, false));
				}
				else
				{
					StoryManager.instance.levelSteps.Enqueue(new LevelStep_Speech("绿色匕首，让我们看看你想干什么。", CharacterManager.instance.head, 2.5f, false));
				}
				StoryManager.instance.levelSteps.Enqueue(new LevelStep_Animation(value.spine, "Run", true, 0f, true));
				StoryManager.instance.levelSteps.Enqueue(new LevelStep_NpcEdit(value, 2, -1f));
				StoryManager.instance.levelSteps.Enqueue(new LevelStep_Tutorial(false, true, this.type, 2f, 1));
				StoryManager.instance.levelSteps.Enqueue(new LevelStep_Wait(1f));
				StoryManager.instance.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, Vector3.zero));
				return true;
			}
			if (TipManager.instance.charCount >= TipManager.instance.targetString.Count - 1 && value.markedForKill && Time.timeScale > 0f && CharacterManager.instance.currentState == CharacterManager.State.Idle)
			{
				Time.timeScale = 0f;
				TipManager.instance.Close(true);
				if (this.type == 2)
				{
					if (value.hitSides.Count == 2)
					{
						TipManager.instance.Open("#点击 #右 攻击！", false, 0f);
					}
					else
					{
						TipManager.instance.Open("他闪避了！ \n 再次#点击 #右 干掉他。", false, 0f);
					}
				}
				else if (value.hitSides.Count == 2)
				{
					TipManager.instance.Open("#点击 #右 攻击！", false, 0f);
				}
				else
				{
					TipManager.instance.Open("他冲到了你身后！\n #点击 #左 干掉他。", false, 0f);
				}
			}
			if (Time.timeScale == 0f && TipManager.instance.charCount >= TipManager.instance.targetString.Count - 1 && Input.GetMouseButtonDown(0) && Input.mousePosition.x > (float)(Screen.width / 2) == (value.side == 1))
			{
				Time.timeScale = 1f;
				CharacterManager.instance.inputQueue.Enqueue(value.side);
				TipManager.instance.Close(true);
				if (value.hitSides.Count == 1)
				{
					TipManager.instance.Open("完美！小心剩下的人。", true, 3f);
					return true;
				}
			}
		}
		return false;
	}
}
