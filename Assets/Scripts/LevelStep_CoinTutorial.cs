// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_CoinTutorial : LevelStep
{
	public int step = -1;

	private int coinsToReach;

	public override void Start()
	{
		base.Start();
		this.ChangeStep(0);
	}

	public override bool Update()
	{
		base.Update();
		switch (this.step)
		{
		case 0:
		case 1:
		case 3:
		case 4:
		case 7:
			if (!TipManager.instance.isOpen)
			{
				this.ChangeStep(this.step + 1);
			}
			else if (TipManager.instance.charCount >= TipManager.instance.targetString.Count - 1 && Input.GetMouseButtonDown(0))
			{
				TipManager.instance.Close(false);
			}
			break;
		case 2:
			if (MonetizationManager.instance.coinSide == -1)
			{
				this.ChangeStep(3);
			}
			break;
		case 5:
			if (MonetizationManager.instance.coins >= this.coinsToReach)
			{
				this.ChangeStep(7);
			}
			break;
		case 8:
			return true;
		}
		return false;
	}

	public void ChangeStep(int newStep)
	{
		switch (newStep)
		{
		case 0:
			TipManager.instance.Close(false);
			TipManager.instance.Open("当你的攻击没有命中敌人，你会短暂眩晕。", true, 6f);
			break;
		case 1:
			TipManager.instance.Close(false);
			TipManager.instance.Open("但是有时候你需要这样做来#收集硬币 。", true, 6f);
			break;
		case 2:
			TipManager.instance.Close(false);
			TipManager.instance.Open("#收集 这枚硬币。", false, 0f);
			SceneManager.instance.inputStarted = true;
			MonetizationManager.instance.SpawnCoin(1);
			break;
		case 3:
			SceneManager.instance.inputStarted = false;
			TipManager.instance.Close(false);
			TipManager.instance.Open("当你收集了一枚硬币，立刻会出现另一枚。", true, 6f);
			break;
		case 4:
			TipManager.instance.Close(false);
			TipManager.instance.Open("在硬币消失前收集它来开启#硬币连击 。", true, 6f);
			break;
		case 5:
			TipManager.instance.Close(false);
			TipManager.instance.Open("#收集4枚硬币 才可通过这个阶段。", false, 0f);
			SceneManager.instance.inputStarted = true;
			this.coinsToReach = MonetizationManager.instance.coins + 4;
			break;
		case 7:
			TipManager.instance.Close(false);
			SceneManager.instance.inputStarted = false;
			TipManager.instance.Open("很好！ 别忘了：每100枚硬币就能让你获得一个#史诗物品 。", true, 6f);
			break;
		}
		this.step = newStep;
	}
}
