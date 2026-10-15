// 反编译自程序集： Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelStep_SummonNpc : LevelStep
{
	public Vector3 summonPos;

	public Color color;

	public int side;

	public float waitTime;

	public float scale;

	private bool isMiniboss;

	private string hatString;

	private string weaponString;

	private Npc miniBoss;

	public LevelStep_SummonNpc(Color color, int side, float waitTime, float scale, bool isMiniboss = false, string weaponString = "fist0", string hatString = "hat0")
	{
		this.hatString = hatString;
		this.weaponString = weaponString;
		this.color = color;
		this.side = side;
		this.waitTime = waitTime;
		this.scale = scale;
		this.isMiniboss = isMiniboss;
		if (isMiniboss)
		{
			this.miniBoss = NpcManager.instance.SpawnEnemy(1, 20f, 2f, 0);
			StoryManager.instance.miniboss = this.miniBoss;
		}
	}

	public override void Start()
	{
		this.summonPos = new Vector3(1f, 0f, 0f) + new Vector3(StoryManager.instance.GetWorldPositionOnPlane(new Vector3((float)Screen.width, 0f, 0f)).x, CharacterManager.instance.transform.position.y, CharacterManager.instance.transform.position.z);
		base.Start();
		if (this.isMiniboss)
		{
			this.miniBoss.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
			this.miniBoss.meshRenderer.material.SetColor("_Color", this.color);
			this.miniBoss.spine.skeleton.flipX = (this.side == -1);
			this.miniBoss.transform.position = this.summonPos;
			this.miniBoss.speed = 0f;
			this.miniBoss.currentHat = ItemManager.instance.hatsById[this.hatString];
			this.miniBoss.currentWeapon = ItemManager.instance.weaponsById[this.weaponString];
			this.miniBoss.EquipHat();
			this.miniBoss.EquipWeapon();
			this.miniBoss.weapon1Sprite.color = Color.black;
			this.miniBoss.weapon2Sprite.color = Color.black;
			this.miniBoss.weapon2hSprite.color = Color.black;
			this.miniBoss.hatSprite.color = Color.black;
		}
	}

	public override bool Update()
	{
		base.Update();
		if (this.miniBoss)
		{
			if (this.miniBoss.meshRenderer.material.color != this.color)
			{
				this.miniBoss.meshRenderer.material.SetColor("_Color", this.color);
			}
		}
		return this.startTime + this.waitTime <= Time.unscaledTime;
	}
}
