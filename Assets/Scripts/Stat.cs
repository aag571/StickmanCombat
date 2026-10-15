// 反编译自程序集： Assembly-CSharp.dll
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class Stat
{
	public int id;

	public string description;

	public ObscuredInt value;

	public string suffix;

	public StatContainer statContainer;

	public Stat(int id, string description, string suffix)
	{
		this.description = description;
		this.id = id;
		this.suffix = suffix;
	}

	public void UpdateStat(int newValue)
	{
		if (this.id >= 3 && this.id <= 6)
		{
			return;
		}
		this.value = newValue;
        if (this.statContainer == null) return;
		if (this.id == 1)
		{
			this.statContainer.valueText.text = ItemManager.instance.LevelToBeltString(newValue) + "带";
		}
		else
		{
			this.statContainer.valueText.text = this.value + this.suffix;
		}
	}
}
