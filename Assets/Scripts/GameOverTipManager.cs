// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class GameOverTipManager : MonoBehaviour
{
	public static GameOverTipManager instance;

	private string[] tips;

	private int lastTipIndex;

	private void Awake()
	{
		GameOverTipManager.instance = this;
	}

	private void Start()
	{
		this.lastTipIndex = PlayerPrefs.GetInt("lastTipIndex", 0);
		this.tips = new string[9];
		this.tips[0] = "完成所有任务，即可解锁下一场腰带考核";
		this.tips[1] = "腰带考核失败后，可以消耗灵魂再次挑战";
		this.tips[2] = "移动时若未击中敌人或拾取灵魂，会短暂眩晕";
		this.tips[3] = "仅当移动无法击中敌人时，拾取灵魂才会使你停下";
		this.tips[4] = "在灵魂消失前连续拾取，可累积灵魂连击";
		this.tips[5] = "拾取灵魂和灵魂连击不会影响得分";
		this.tips[6] = "每通过一次腰带考核，就能解锁考官的帽子和武器";
		this.tips[7] = "每通过一次腰带考核，就能解锁考官的帽子和武器";
		this.tips[8] = "仅当移动无法击中敌人时，拾取灵魂才会使你停下";
	}

	public string GetNextTip()
	{
		this.lastTipIndex++;
		if (this.lastTipIndex > this.tips.Length)
		{
			this.lastTipIndex = 1;
		}
		return this.tips[this.lastTipIndex - 1];
	}

	public void Save()
	{
		PlayerPrefs.SetInt("lastTipIndex", this.lastTipIndex);
	}
}
