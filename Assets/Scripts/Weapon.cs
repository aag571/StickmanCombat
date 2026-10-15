// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon
{
    private string _EnglishName_k__BackingField;

    private float _price_k__BackingField;

    private int _unlockLevel_k__BackingField;

    public WeaponCategory category;

    public int index;

    public int totalIndex;

    public string id;

    public Sprite sprite;

    public Vector2 startPos;

    public Vector2 endPos;

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

	public float price
	{
		get;
		set;
	}

	public int unlockLevel
	{
		get;
		set;
	}

	public void Instantiate(WeaponCategory category, int index)
	{
		this.index = index;
		this.category = category;
		this.id = category.id.ToLower() + index;
		if (category.index == 0 && this.index == 0)
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
		this.sprite = Resources.Load<Sprite>("Items/Weapons/" + this.id);
		this.startPos = new Vector2((this.sprite.rect.width - this.sprite.border.z) / 100f - this.sprite.pivot.x / 100f, this.sprite.border.y / 100f - this.sprite.pivot.y / 100f);
		this.endPos = new Vector2(this.sprite.border.x / 100f - this.sprite.pivot.x / 100f, (this.sprite.rect.height - this.sprite.border.w) / 100f - this.sprite.pivot.y / 100f);
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
