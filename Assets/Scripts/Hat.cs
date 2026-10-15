// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hat
{
	private string _EnglishName_k__BackingField;

	private string _id_k__BackingField;

	private int _unlockLevel_k__BackingField;

	public int index;

	public Sprite sprite;

	public bool isOwned;

	public string EnglishName
	{
		get;
		set;
	}

    public string ChineseName
    {
        get;
        set;
    }

	public string id
	{
		get;
		set;
	}

	public int unlockLevel
	{
		get;
		set;
	}

	public void Instantiate(int index)
	{
		if (index == 0)
		{
			this.isOwned = true;
		}
		else
		{
			this.isOwned = PlayerPrefsX.GetBool("is" + this.id + "Owned", false);
		}
		if (this.isOwned)
		{
			ItemManager.instance.itemCount++;
		}
		this.index = index;
		this.sprite = Resources.Load<Sprite>("Items/Hats/" + this.id);
	}

	public string GetName()
	{
		return string.IsNullOrEmpty(this.ChineseName) ? this.EnglishName : this.ChineseName;
	}

    public string GetChineseName()
    {
        return this.ChineseName;
    }

	public void Own()
	{
		ItemManager.instance.itemCount++;
		this.isOwned = true;
        PlayerPrefsX.SetBool("is" + this.id + "Owned", true);
	}

	public bool IsOwned()
	{
        if (StickmanCombat.Gameplay.GameFlowController.instance != null) return ItemManager.instance.allItemsBought || isOwned;
		return (ItemManager.instance.allItemsBought || this.isOwned) && (this.unlockLevel <= 0 || this.unlockLevel <= StoryManager.instance.level);
	}
}
